namespace magaApp.ViewModels
{
    public class ExplorerViewModel
    {
        public int IdUser { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

       
        public string Initials
        {
            get
            {
                if (string.IsNullOrEmpty(FullName))
                    return "";

                var names = FullName.Split(' ');
                var initials = names.Length > 1 ? $"{names[0][0]}{names[1][0]}" : names[0].Substring(0, 1);
                return initials.ToUpper();
            }
        }
    }
}
