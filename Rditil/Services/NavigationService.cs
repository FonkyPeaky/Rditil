using System.Windows;

namespace Rditil.Services
{
    public static class NavigationService
    {
        /// <summary>
        /// Ferme la fenêtre actuelle et ouvre une nouvelle fenêtre.
        /// </summary>
        /// <param name="current">Fenêtre actuelle</param>
        /// <param name="next">Nouvelle fenêtre à afficher</param>
        public static void NavigateTo(Window current, Window next)
        {
            next.Show();
            current.Close();
        }
    }
}
