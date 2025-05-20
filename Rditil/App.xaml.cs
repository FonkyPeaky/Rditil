using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Rditil.Data;
using Rditil.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Rditil
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Utilisateur? CurrentUser { get; set; }
    }

}
