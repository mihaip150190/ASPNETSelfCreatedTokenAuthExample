// Write your Javascript code.
(function () {
    angular.module('SimpleRESTWebsite', ['LocalStorageModule', 'ui.router'])
        .constant('ENDPOINT_URI', 'http://localhost:5092/')
        .config(function($stateProvider, $urlRouterProvider, $httpProvider) {
                $stateProvider
                    .state('login', {
                        url: '/login',
                        templateUrl: 'app/templates/login.tmpl.html',
                        controller: 'LoginCtrl',
                        controllerAs: 'login'
                    })
                    .state('dashboard', {
                        url: '/dashboard',
                        templateUrl: 'app/templates/dashboard.tmpl.html'
                    });;

                $urlRouterProvider.otherwise('/dashboard');

            $httpProvider.interceptors.push('APIInterceptor');
        })
        .service('APIInterceptor', function ($rootScope, UserService) {
            var service = this;
            service.request = function (config) {
                var currentUser = UserService.getCurrentUser(),
                    access_token = currentUser ? currentUser.token : null;

                if (access_token) {
                    config.headers.authorization = 'Bearer ' + access_token;
                }
                return config;
            };

            service.responseError = function (response) {
                if (response.status === 401) {
                    $rootScope.$broadcast('unauthorized');
                }
                return response;
            };
        })
        .service('UserService', function (localStorageService) {
           var service = this,
               currentUser = null;

           service.setCurrentUser = function (user) {
               currentUser = user;
               localStorageService.set('user', user);
               return currentUser;
           };

           service.getCurrentUser = function () {
               if (!currentUser) {
                   currentUser = localStorageService.get('user');
               }
               return currentUser;
           };
       })
        .service('LoginService', function ($http, $state,ENDPOINT_URI) {
            var service = this;
               
            service.login = function (credentials) {
                return $http.post(ENDPOINT_URI + 'api/Token', credentials);
            };

            service.logout = function () {
                $state.go('login');
            };

            service.register = function (user) {
                return $http.post(ENDPOINT_URI + 'Account/Register', user);
            };
        })
        .service('ValuesService', function ($http, ENDPOINT_URI) {
            var service = this;

            service.adminOnly = function () {
                return $http.get(ENDPOINT_URI + 'api/Admin');
            };

            service.visitorOnly = function () {
                return $http.get(ENDPOINT_URI + 'api/Visitor');
            };

            service.anonymous = function () {
                return $http.get(ENDPOINT_URI + 'api/Values');
            }
        })
        .controller('LoginCtrl', function ($rootScope, $state, LoginService, UserService) {
            var login = this;

            function signIn(user) {
                LoginService.login(user)
                    .then(function (response) {
                        if (response.data && response.data.authenticated && response.data.token) {
                            user.token = response.data.token;
                            UserService.setCurrentUser(user);
                            $rootScope.$broadcast('authorized');
                            $state.go('dashboard');
                        }
                        else {
                            alert('An error occurred. Please try again');
                            $rootScope.$broadcast('unauthorized');
                        }                        
                    }, function (error) {
                        console.log(error);
                    });
            }

            function register(user) {
                debugger;
                user.ConfirmPassword = user.Password;
                LoginService.register(user)
                    .then(function (response) {
                        debugger;
                        signIn(user);
                    },function (response) {
                        console.log(response);
                    });
            }

            function submit(user) {
                login.newUser ? register(user) : signIn(user);
            }

            login.newUser = false;
            login.submit = submit;
        })
        .controller('MainCtrl', function ($rootScope, $state, ValuesService, LoginService, UserService ) {
            var main = this;

            function logout() {
                //LoginService.logout()
                //    .then(function (response) {
                //        main.currentUser = UserService.setCurrentUser(null);
                //        $state.go('login');
                //    }, function (error) {
                //        console.log(error);
                //    });
                main.currentUser = UserService.setCurrentUser(null);
                $state.go('login');
            }

            var logResponse = function (response) {
                main.content = JSON.stringify(response);
            }

            function anonymous() {
                debugger;
                ValuesService.anonymous().then(logResponse);
            }

            function onlyAdmin() {
                ValuesService.adminOnly().then(logResponse);
            }

            function onlyVisitor() {
                ValuesService.visitorOnly().then(logResponse);
            }

            $rootScope.$on('authorized', function () {
                main.currentUser = UserService.getCurrentUser();
            });

            $rootScope.$on('unauthorized', function () {
                main.currentUser = UserService.setCurrentUser(null);
                $state.go('login');
            });

            main.anonymous = anonymous;
            main.onlyAdmin = onlyAdmin;
            main.onlyVisitor = onlyVisitor;
            main.logout = logout;
            main.currentUser = UserService.getCurrentUser();
            main.content = 'Output from action will go here';
        })
        ;
})();
