using rinconLosCuatroTPV.MVVM.View;
using rinconLosCuatroTPVDesktop.MVVM.ViewModels;
using Syncfusion.Licensing;

namespace RinconLosCuatroTPVDesktop
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF1cXmhPYVFwWmFZfVpgdV9GYFZQR2Y/P1ZhSXxXdkBhXn5dcHRUQ2VbVEc=");
            MainPage = new NavigationPage(new Ventana(new ViewModel()));
        }
    }
}
