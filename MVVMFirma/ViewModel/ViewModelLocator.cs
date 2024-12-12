using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using MVVMFirma.ViewModels;

namespace MVVMFirma.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Rejestracja ViewModeli
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AllProductsViewModel>();
            SimpleIoc.Default.Register<NewProductViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public AllProductsViewModel AllProducts => ServiceLocator.Current.GetInstance<AllProductsViewModel>();
        public NewProductViewModel NewProduct => ServiceLocator.Current.GetInstance<NewProductViewModel>();

        public static void Cleanup()
        {
            // TODO: Czyœæ instancje ViewModeli, jeœli to konieczne
        }
    }
}
