namespace MyApp

type MainViewModel () =
  let welcome = WelcomeViewModel ()

  member vm.Welcome
    with get() = welcome