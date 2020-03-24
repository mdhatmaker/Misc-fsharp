namespace WpfDemo


open WpfDemo.Library

type MainViewModel () =
    let welcomeTextProvider = WelcomeTextProvider ()

    member vm.WelcomeText
        with get() = welcomeTextProvider.GetWelcomeText ()



