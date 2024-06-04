using PropertyChanged;
using rinconLosCuatroTPVDesktop.MVVM.Models;
using rinconLosCuatroTPVDesktop.MVVM.View;
using rinconLosCuatroTPVDesktop.MVVM.ViewModels;

namespace rinconLosCuatroTPV.MVVM.View;
[AddINotifyPropertyChangedInterface]
public partial class Ventana : TabbedPage
{
	ViewModel viewModel;
	public Ventana(ViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }


    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        this.viewModel.filterProducts(e.NewTextValue);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        viewModel.isEditMode = false;
        await Navigation.PushModalAsync(new AnadirOModificarView(this.viewModel));
    }

    private void Button_Clicked_Delete(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var producto = (Producto)button.BindingContext;

        viewModel.deleteProduct(producto);
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        var switchToggled = (Button)sender;
        var producto = (Producto)switchToggled.BindingContext;
        producto.Availability = !producto.Availability;

        viewModel.changeAvailability(producto);
    }

    private void Button_Clicked_Update(object sender, EventArgs e)
    {
        viewModel.isEditMode = true;
        var button = (Button)sender;
        var producto = (Producto)button.BindingContext;
        viewModel.ActualProduct = producto;
        Navigation.PushModalAsync(new AnadirOModificarView(this.viewModel),true);
    }

    private void Button_Clicked_Strike(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        Grid grid = (Grid)button.Parent;

        Image image = grid.FindByName<Image>("CheckmarkIcon");
        image.IsVisible = !image.IsVisible;
    }
}