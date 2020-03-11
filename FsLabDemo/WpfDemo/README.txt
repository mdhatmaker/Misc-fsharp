WpfDemo (F# Project)

demonstrates using WPF with F#
https://fslab.org/

This project contains only Scripts (fsx files) - most of which
are taken from or related to the FsLabDemo project.

Some of this code will be destined for the "Code Graveyard", while
some of it will provide looks at other FsLab functionality or the
same FsLab functionality covered in FsLabDemo, but designed for
execution in the F# Immediate window.





MISC REFERENCES

https://www.c-sharpcorner.com/article/create-wpf-application-with-f-sharp-and-fsxaml/
http://putridparrot.com/blog/creating-a-wpf-application-in-f/
https://docs.microsoft.com/en-us/archive/msdn-magazine/2011/september/fsharp-programming-build-mvvm-applications-in-fsharp
http://putridparrot.com/blog/f-mvvm-plumbing-code/
https://github.com/fsprojects/FsXaml
https://alexatnet.com/quickstart-wpf-f-only-app-in-vscode-part-2/
http://stevenpemberton.net/blog/2015/03/29/FSharp-WPF-and-the-XAML-type-provider/
https://www.c-sharpcorner.com/technologies/f-sharp









        </local:Welcome>
        <TextBlock Text="Chart Demos"  
                   HorizontalAlignment="Center"  
                   VerticalAlignment="Center" Margin="16,8,274.333,246.667" />
        <Button x:Name="LaunchDemoButton" Content="Launch" HorizontalAlignment="Left" Margin="28,234,0,0" VerticalAlignment="Top" Width="75" RenderTransformOrigin="0.061,-0.645" Click="LaunchDemoButton_Click"/>
        <ListBox x:Name="ChartDemoList" HorizontalAlignment="Left" Height="194" Margin="16,29,0,0" VerticalAlignment="Top" Width="100"/>
        <TextBlock Text="LiveCharts"  
            HorizontalAlignment="Center"  
            VerticalAlignment="Center" Margin="155,8,168.333,246.667" />
        <Button x:Name="LaunchLiveChartButton" Content="Launch" HorizontalAlignment="Left" Margin="150,234,0,0" VerticalAlignment="Top" Width="75" RenderTransformOrigin="0.061,-0.645"/>
        <ListBox x:Name="LiveChartList" HorizontalAlignment="Left" Height="194" Margin="138,29,0,0" VerticalAlignment="Top" Width="100"/>
        <local:Welcome DataContext="{Binding Welcome}"/>
    </Grid>