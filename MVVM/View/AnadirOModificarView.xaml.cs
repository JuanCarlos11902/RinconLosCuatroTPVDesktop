using rinconLosCuatroTPVDesktop.MVVM.Models;
using rinconLosCuatroTPVDesktop.MVVM.ViewModels;
using System.Net.Http.Headers;

namespace rinconLosCuatroTPVDesktop.MVVM.View;

public partial class AnadirOModificarView : ContentPage
{
	ViewModels.ViewModel viewModel;
    private string filePath;
    public AnadirOModificarView(ViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel = viewModel;
        BindingContext = this.viewModel;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            FileTypes = FilePickerFileType.Images,
            PickerTitle = "Selecciona una Imagen"
        });

        if (result != null)
        {
            var stream = await result.OpenReadAsync();
            ImageSourcePicked.Source = ImageSource.FromStream(() => stream);
            filePath = result.FullPath;
        }


    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {

        Navigation.PopModalAsync();
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        Producto producto = viewModel.ActualProduct;
        string name = NameEntry.Text;
        string description = DescriptionEntry.Text;
        Double price = Convert.ToDouble(PriceEntry.Text);
        bool availability = AvailabilitySwitch.IsToggled;
        string type;
        try
        {
            type = TypePicker.SelectedItem.ToString();
        }
        catch (NullReferenceException exception)
        {
            type = "";
            Console.WriteLine(exception);
        }
        if (!viewModel.isEditMode)
        {
            viewModel.addProduct(name, description, price, availability, type, filePath);
        }
        else
        {
            viewModel.updateProduct(producto, name, description, price, availability, type, filePath);
        }
    }
}