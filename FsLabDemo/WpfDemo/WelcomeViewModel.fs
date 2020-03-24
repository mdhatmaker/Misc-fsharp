namespace WpfDemo

open WpfDemo.Library

type WelcomeViewModel () =
    let welcomeTextProvider = WelcomeTextProvider ()

    member vm.WelcomeText
        with get() = welcomeTextProvider.GetWelcomeText ()




