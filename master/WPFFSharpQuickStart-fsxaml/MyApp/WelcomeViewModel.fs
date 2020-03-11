namespace MyApp

open MyApp.Library

type WelcomeViewModel () =
  let welcomeTextProvider = WelcomeTextProvider ()

  member vm.WelcomeText
    with get() = welcomeTextProvider.GetWelcomeText ()