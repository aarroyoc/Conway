/* Arroyo Calle, Adrián
Bazaco Velasco, Daniel*/
using Avalonia;
using Avalonia.Markup.Xaml;

namespace Conway
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}