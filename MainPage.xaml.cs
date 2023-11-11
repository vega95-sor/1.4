using Ejercicio1_4.Models;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ejercicio1_4
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        byte[] imageToSave;

        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            var takepic = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "PhotoApp",
                Name = DateTime.Now.ToString() + "_foto.jpg",
                SaveToAlbum = true
            });

           // await DisplayAlert("Ubicacion de la foto: ", takepic.Path, "Ok");

            if (takepic != null)
            {
                imageToSave = null;
                MemoryStream memoryStream = new MemoryStream();

                takepic.GetStream().CopyTo(memoryStream);
                imageToSave = memoryStream.ToArray();

                img.Source = ImageSource.FromStream(() => { return takepic.GetStream(); });
            }


        }

        private async void btnSQlite_Clicked(object sender, EventArgs e)
        {
            var empleado = new Empleados
            {
                nombres = txtnombre.Text,
                descripcion = txtdescripcion.Text,
                imagen = imageToSave
            };

            var resultado = await App.BaseDatos.EmpleSave(empleado);

            if (resultado != 0)
            {
                await DisplayAlert("Atencion", "Persona ingresada correctamente!!!", "Ok");
            }
            else
            {
                await DisplayAlert("Atencion", "Upps ha ocurrido un error inesperado", "Ok");
            }


            await Navigation.PopAsync();
        }

        private byte[] GetImageBytes(Stream stream)
        {
            byte[] ImageBytes;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                ImageBytes = memoryStream.ToArray();
            }
            return ImageBytes;
        }
    }
}
