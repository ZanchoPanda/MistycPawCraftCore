using System.Windows;
using System.Windows.Controls;

namespace MistycPawCraftCore.Utils.Helpers
{
    public static class WebBrowserHelper
    {

        public static readonly DependencyProperty BindableSourceProperty =
        DependencyProperty.RegisterAttached(
            "BindableSource",
            typeof(string),
            typeof(WebBrowserHelper),
            new UIPropertyMetadata(null, OnBindableSourceChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        private static void OnBindableSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var browser = d as WebBrowser;
            if (browser != null && e.NewValue is string url)
            {
                browser.Navigate(url);
            }
        }

    }
}
