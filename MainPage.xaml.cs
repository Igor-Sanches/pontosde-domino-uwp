using System;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// App Pontos de dominó programado em C# por Igor De Jesus Dutra Sanches em Picada/Anajatuba - MA // Projeto iniciado no dia 20/01/2019 para uso próprio em partidas de dominó como um placar eletrónico

namespace Pontos_de_Domino_UWP
{
    #region App Pontos de dominó programado em C# 
    public sealed partial class MainPage : Page
    {
        //O Placar A e Barmagena todos dados e retorna os pontos
        double PlacarA = 0;
        double PlacarB = 0;

        public MainPage()
        {
            this.InitializeComponent();
            //Deixa o botão "Nova partida" dispoivel ou não de acordo com o placar 
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += MainPage_BackPressed;
            ButtonZero();
        }

        private void MainPage_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (paneH.IsPaneOpen) paneH.IsPaneOpen = !paneH.IsPaneOpen;
            if (paneI.IsPaneOpen) paneI.IsPaneOpen = !paneI.IsPaneOpen;
        }

        #region IsPane e Lista
        //O IsPane retorna se a Lista é dos salvos ou guardados
        bool IsPane;
        bool Lista => IsPane;
        #endregion

        private static ApplicationDataContainer container = ApplicationData.Current.LocalSettings;

        public static void PutBool(string Key, bool Value)
        {
            if (!container.Values.ContainsKey(Key)) container.Values.Add(Key, Value); else container.Values[Key] = Value;
        }
        static bool _bool;
        public static bool GetBool(string Key, bool Default)
        {
            if (container.Values.ContainsKey(Key)) _bool = (bool)container.Values[Key]; else _bool = Default;
            return _bool;
        }


        #region Menu Flyout => O menu com as opções salvar, listar e sobre
        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem i = (MenuFlyoutItem)sender as MenuFlyoutItem;
            switch (i.Tag.ToString())
            {
                case "a1":
                    if(GetBool("saveIsAuto", false))
                    {
                        AddSalvosDB();
                    }
                    else
                    {
                        TextBlock text = new TextBlock();
                        CheckBox box = new CheckBox();
                        StackPanel painel = new StackPanel();
                        text.Text = loader.GetString("saveinautosave");
                        text.TextWrapping = TextWrapping.Wrap;
                        box.Content = loader.GetString("Nmostrarnovamente");
                        box.Margin = new Thickness(0, 10, 0, 0);
                        box.IsChecked = GetBool("saveIsAuto", false);
                        box.Checked += (iii, ii) => { PutBool("saveIsAuto", box.IsChecked.Value); };
                        painel.Children.Add(text);
                        painel.Children.Add(box);
                        Grid grid = new Grid();
                        grid.Children.Add(painel);
                        var d = new ContentDialog()
                        {
                            Title = loader.GetString("pd"),
                            Content = grid,
                            CloseButtonText = loader.GetString("Fechar"),
                            PrimaryButtonText = loader.GetString("Salvar"),
                            RequestedTheme = ElementTheme.Dark,
                            Background = ((SolidColorBrush)App.Current.Resources["Background"]) 
                        };
                        d.PrimaryButtonClick += (s, ef) => { AddSalvosDB(); };
                        d.CloseButtonClick += (s, ef) => { };
                        await d.ShowAsync();
                    }
                    break;
                case "a2":
                    UpdateMenu();
                    paneH.IsPaneOpen = !paneH.IsPaneOpen;
                        break; 
                    break;
                case "a4":
                    paneI.IsPaneOpen = !paneI.IsPaneOpen;
                    break;
                case "a5":
                    MensagemError($"{loader.GetString("Aplicativocriadopor")} Igor de Jesus Dutra Sanches {loader.GetString("em")} Picada, Anajatuba - MA.");
                    break;
            }
        }
        #endregion

        #region Atualiza os Botões para mostrar se a lista é salvos ou guardados
        private void UpdateMenu()
        {
            list.ItemsSource = GetSalvos.Lista().OrderByDescending(x => x.pontosA);  
            lista10.Visibility = Visibility.Collapsed; 
            ListaVazia.Visibility = Visibility.Collapsed;
            Delete.IsEnabled = false;
            if (ListaItemsCount == 0)
            {
                ListaVazia.Visibility = Visibility.Visible;
                lista10.Visibility = Visibility.Collapsed; 
                Delete.IsEnabled = false;
            }
            else
            { 
                    lista10.Visibility = Visibility.Visible; 
                    ListaVazia.Visibility = Visibility.Collapsed;
                    Delete.IsEnabled = true;
               
            }
        }
        #endregion

        #region Banco de dados
        db.Banco_De_Dados.Classes.SalvosController GetSalvos = new db.Banco_De_Dados.Classes.SalvosController(); 
        #endregion

        #region Retorna o número de items no banco de dados dos salvos e guardados
        long ListaItemsCount
        {
            get
            {
                 return GetSalvos.Lista().Count; 
            }
        }
        #endregion

        #region Button para zera os placares
        private void Zera_Click(object sender, RoutedEventArgs e)
        {
            ZerarTudo();
        }
        #endregion

        #region Salva e edita um error caso o usuário digite um valor errado e retorna o valor editado para placar
        private void SaveAndEditA_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender as Button;
            if (btn.Tag.ToString() == "save")
            {
                SaveA();
            }
            else
            {
                SaveAndEditA.Visibility = Visibility.Collapsed;
                EditA.Visibility = Visibility.Visible;
                SaveAndEdit_A.Visibility = Visibility.Visible;
            }
        }

        ResourceLoader loader = new ResourceLoader();

        private void SaveA()
        {
            try
            {
                if (EditA.Text == "") loader.GetString("Voceprecisadigitaralgumnumero");
                else
                {
                    if (EditA.Text != "0")
                    {
                        if (EditA.Text.StartsWith("0")) loader.GetString("Naoepossiveliniciarcomzerodigiteoutrovalor");
                        else
                        {
                            InitA();
                        }
                    }
                    else
                    {
                        InitA();
                    }

                }
                EditA.Visibility = Visibility.Collapsed;
                SaveAndEdit_A.Visibility = Visibility.Collapsed;
                SaveAndEditA.Visibility = Visibility.Visible;
            }
            catch { }
        }
        private void InitA()
        {
            PontosA.Text = EditA.Text;
            PlacarA = Convert.ToDouble(PontosA.Text);
            Is20Pontos(1);
        }
        
        private void SaveAndEditB_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender as Button;
            if (btn.Tag.ToString() == "save")
            {
                SaveB();
            }
            else
            {
                SaveAndEditB.Visibility = Visibility.Collapsed;
                EditB.Visibility = Visibility.Visible;
                SaveAndEdit_B.Visibility = Visibility.Visible;
            }
        }
        private void SaveB()
        {
            try
            {
                if (EditB.Text == "") loader.GetString("Voceprecisadigitaralgumnumero");
                else
                {
                    if (EditB.Text != "0")
                    {
                        if (EditB.Text.StartsWith("0")) loader.GetString("Naoepossiveliniciarcomzerodigiteoutrovalor");
                        else
                        {
                            InitB();
                        }
                    }
                    else
                    {
                        InitB();
                    }

                }
                EditB.Visibility = Visibility.Collapsed;
                SaveAndEdit_B.Visibility = Visibility.Collapsed;
                SaveAndEditB.Visibility = Visibility.Visible;
            }
            catch { }
        }
        private void InitB()
        {
            PontosB.Text = EditB.Text;
            PlacarB = Convert.ToDouble(PontosB.Text);
            Is20Pontos(2);
        }
        #endregion

        #region Caixa de mensagem
        /// <summary>
        /// Retorna uma mensagem
        /// </summary>
        /// <param name="v"></param>
        private async void MensagemError(string v)
        {
            var d = new ContentDialog()
            {
                Title = loader.GetString("pd"),
                Content = v,
                CloseButtonText = loader.GetString("Fechar"),
                RequestedTheme = ElementTheme.Dark,
                Background = ((SolidColorBrush)App.Current.Resources["Background"])

            };
            d.CloseButtonClick += (s, e) => {   }; 
            await d.ShowAsync();
        }
        #endregion

        private void ClickButtonsA_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender as Button;
            switch (btn.Tag.ToString())
            {
                case "Passe":
                    CalcularPontosA(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Passe));
                    break;
                case "Passede2":
                    CalcularPontosA(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.PasseDeDois));
                    break;
                case "Passedesaida":
                    CalcularPontosA(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.passeDeSaida));
                    break;
                case "Passede2saida":
                    CalcularPontosA(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.passe2Saida));
                    break;
                case "Geral":
                    CalcularPontosA(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Geral));
                    break;
                case "Batida":
                    CalcularPontosA(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Batida));
                    break;
                case "Geralinconsciente":
                    CalcularPontosB(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.GeralInconsiente));
                    break;
                case "Batidalascada":
                    CalcularPontosA(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Lascado));
                    break; 
                case "Batidadecamburão":
                    CalcularPontosA(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Camburao));
                    break; 
            }
        }
        private string NomesDosJogadores(int x)
        {
            if (x == 1)
            {
                if (JogdoresA.Text != "") return JogdoresA.Text;
                else return JogdoresA.PlaceholderText;
            }
            else
            {
                if (JogdoresB.Text != "") return JogdoresB.Text;
                else return JogdoresB.PlaceholderText;
            }
        }
        private void CalcularPontosA(double v)
        {
            PlacarA = PlacarA + v;
            PontosA.Text = PlacarA.ToString();
            EditA.Text = PontosA.Text;

            Is20Pontos(1);
        }

        private void Is20Pontos(int v)
        {
            if (_20M.IsChecked.Value) MessageDialog(v);
            else
            {
                if (PlacarA >= 20 || PlacarB >= 20) _20M.IsEnabled = false;
                else { _20M.IsEnabled = true; }
            }
            CaraUpdate();

        }

        string nome;
        double ponto1, ponto2;
        private async void MessageDialog(int x)
        {
            if (PlacarA >= 20 || PlacarB >= 20)
            {
                if (x == 1)
                {
                    nome = NomesDosJogadores(1);
                    ponto1 = PlacarA;
                    ponto2 = PlacarB;
                }
                else
                {
                    nome = NomesDosJogadores(2);
                    ponto1 = PlacarB;
                    ponto2 = PlacarA;
                }
                var d = new ContentDialog()
                {
                    Title = loader.GetString("fimdegame"),
                    Content = $"{nome} {loader.GetString("ganharaode")} {ponto1} a {ponto2}, {loader.GetString("inicieumanovapartidaoucontinueamesma")}",
                    PrimaryButtonText = loader.GetString("Iniciarnovapartida"),
                    SecondaryButtonText = loader.GetString("Continuarpartida"),
                    RequestedTheme = ElementTheme.Dark,
                    Background = ((SolidColorBrush)App.Current.Resources["Background"])


                };
                d.PrimaryButtonClick += (s, e) => { ZerarTudo(); };
                d.SecondaryButtonClick += (s, e) => { _20M.IsChecked = false; _20M.IsEnabled = false; CaraUpdate(); };
                await d.ShowAsync();
            }
          
        }

        private void ZerarTudo()
        { 
            PlacarA = 0; PlacarB = 0;
            PontosA.Text = PlacarA.ToString();
            EditA.Text = PontosA.Text;
            PontosB.Text = PlacarB.ToString();
            EditB.Text = PontosB.Text;
            _20M.IsEnabled = true;
            _2X.IsChecked = false;
            CaraUpdate();
            ButtonZero();
        }

        private void CaraUpdate()
        {
            caraA.Text = Feliz(PlacarA, PlacarB);
            caraB.Text = Feliz(PlacarB, PlacarA);
            if (PlacarA == PlacarB)
            {
                caraA.Foreground = new SolidColorBrush(Colors.Gray);
                caraB.Foreground = new SolidColorBrush(Colors.Gray); ;
            }
            else if (PlacarA > PlacarB)
            {
                caraA.Foreground = new SolidColorBrush(Colors.Green);
                caraB.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (PlacarB > PlacarA)
            {
                caraA.Foreground = new SolidColorBrush(Colors.Red);
                caraB.Foreground = new SolidColorBrush(Colors.Green);
            }
            ButtonZero();
        }

        private void CalcularPontosB(double v)
        {
            PlacarB = PlacarB + v;
            PontosB.Text = PlacarB.ToString();
            EditB.Text = PontosB.Text;

            Is20Pontos(2);
        }

        string CaraFeliz;
        string Feliz(double x, double x2)
        {
            if (_20M.IsChecked.Value)
            {
                if (x >= x2 + 18) CaraFeliz = "😎";
                else if (x >= x2 + 16) CaraFeliz = "😍";
                else if (x >= x2 + 14) CaraFeliz = "😂";
                else if (x >= x2 + 10) CaraFeliz = "😋";
                else if (x >= x2 + 6) CaraFeliz = "😉"; 
                else if (x <= x2 - 14) CaraFeliz = "😰";
                else if (x <= x2 - 12) CaraFeliz = "😪";
                else if (x <= x2 - 8) CaraFeliz = "😓";
                else if (x <= x2 - 4) CaraFeliz = "😥";
                else CaraFeliz = "☺️";
                return CaraFeliz;
            }
            else
            {
                if (x >= x2 + 100) CaraFeliz = "😎";
                else if (x >= x2 + 60) CaraFeliz = "😍";
                else if (x >= x2 + 40) CaraFeliz = "😂";
                else if (x >= x2 + 10) CaraFeliz = "😋";  
                else if (x <= x2 - 74) CaraFeliz = "😰";
                else if (x <= x2 - 52) CaraFeliz = "😪";
                else if (x <= x2 - 30) CaraFeliz = "😓";
                else if (x <= x2 - 10) CaraFeliz = "😥";
                else CaraFeliz = "☺️";
                return CaraFeliz;
            }

        }

        private void ClickButtonsB_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender as Button;
            switch (btn.Tag.ToString())
            {
                case "Passe":
                    CalcularPontosB(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Passe));
                    break;
                case "Passede2":
                    CalcularPontosB(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.PasseDeDois));
                    break;
                case "Passedesaida":
                    CalcularPontosB(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.passeDeSaida));
                    break;
                case "Passede2saida":
                    CalcularPontosB(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.passe2Saida));
                    break;
                case "Geral":
                    CalcularPontosB(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Geral));
                    break;
                case "Batida":
                    CalcularPontosB(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Batida));
                    break;
                case "Geralinconsciente":
                    CalcularPontosA(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.GeralInconsiente));
                    break;
                case "Batidalascada":
                    CalcularPontosB(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Lascado));
                    break;
                case "Batidadecamburão":
                    CalcularPontosB(TypesCommands.Commands.Command(_2X.IsChecked.Value, TypesCommands.Types.Camburao));
                    break;
            }
        }

        double x1, x2; string n1, n2, _result, x;

        void ButtonZero()
        {
            if (PlacarA == 0 && PlacarB == 0)
            {
                Zera.IsEnabled = false;
                SaveP.IsEnabled = false;
            }
            else
            {
                Zera.IsEnabled = true;
                SaveP.IsEnabled = true;
            }
        }
        
        private void Edit_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if(e.OriginalKey == Windows.System.VirtualKey.Enter) {
                TextBox Box = (TextBox)sender as TextBox;
                switch (Box.Tag.ToString())
                {
                    case "A":
                        SaveA();
                        break;
                    case "B":
                        SaveB();
                        break;
                    case "jA":
                        if(JogdoresA.Text != "")
                        {
                            JogdoresA.PlaceholderText = JogdoresA.Text;
                            JogdoresA.Text = "";
                        }
                        break;
                    case "jB":
                        if (JogdoresB.Text != "")
                        {
                            JogdoresB.PlaceholderText = JogdoresB.Text;
                            JogdoresB.Text = "";
                        }
                        break;
                }
            }
        }

        private async void Contato_Click(object sender, RoutedEventArgs e)
        {
            var d = new ContentDialog()
            {
                Title = loader.GetString("pd"),
                Content = loader.GetString($"ProseguirWhtasapp"),
                PrimaryButtonText = loader.GetString("Sim"),
                CloseButtonText = loader.GetString("Nao"), 
                RequestedTheme = ElementTheme.Dark,
                Background = ((SolidColorBrush)App.Current.Resources["Background"])
                 
            };
            d.PrimaryButtonClick += async (x, c) =>
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri($"https://api.whatsapp.com/send?phone=+5598985356501&text={loader.GetString("vamosconversar")}%20{loader.GetString("pdx")}"));
            };
            await d.ShowAsync();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ListaItemsCount == 0)
            {
                var d = new ContentDialog()
                {
                    Title = loader.GetString("pd"),
                    Content = loader.GetString($"Nadaparaapagarsualistaestavazia"),
                    CloseButtonText = loader.GetString("Fechar"),
                    RequestedTheme = ElementTheme.Dark,
                    Background = ((SolidColorBrush)App.Current.Resources["Background"])


                };
                d.CloseButtonClick += (ss, es) =>
                {
                };
                d.SecondaryButtonClick += (ss, es) => { };
                await d.ShowAsync();
            }
            else
            {
                string x;
                string x2;
                if (ListaItemsCount == 1)
                { x = loader.GetString("partida"); x2 = loader.GetString("Apagarestaunicapartida"); }
                else { x = loader.GetString("partidas"); x2 = loader.GetString("Temcertezequedesejalimpatodaestalista"); }
                var d = new ContentDialog()
                    {
                        Title = $"{loader.GetString("Apagar")} {ListaItemsCount} {x}",
                        Content = x2,
                    CloseButtonText = loader.GetString("Nao"),
                    PrimaryButtonText = loader.GetString("Sim"),
                    RequestedTheme = ElementTheme.Dark,
                        Background = ((SolidColorBrush)App.Current.Resources["Background"])


                    };
                    d.PrimaryButtonClick += (ss, es) =>
                    {
                        DeleteAll();
                    };
                    d.SecondaryButtonClick += (ss, es) => { };
                    await d.ShowAsync();
             }

        }

        private async void DeleteAll()
        {
            try {
                db.Banco_De_Dados._dbConection db = new db.Banco_De_Dados._dbConection();
               

                    db._conexao.DeleteAll<db.Banco_De_Dados.Classes.Salvos>();
                
                paneH.IsPaneOpen = !paneH.IsPaneOpen;
            }catch(Exception x) { await new MessageDialog(x.Message).ShowAsync(); }
        }

        private async void Restaurar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender as Button;
            db.Banco_De_Dados.Classes.Salvos s = btn.DataContext as db.Banco_De_Dados.Classes.Salvos;
           
            var d = new ContentDialog()
            {
                Title = loader.GetString("pd"),
                Content = loader.GetString($"Desejaretornarestapartida"),
                PrimaryButtonText = loader.GetString("Retornarpartida"),
                SecondaryButtonText = loader.GetString("Fechar"),
                RequestedTheme = ElementTheme.Dark,
                Background = ((SolidColorBrush)App.Current.Resources["Background"])


            };
            d.PrimaryButtonClick += (ss, es) => {
               
                ZerarTudo();
                PlacarA = s.pontosA;
                PontosA.Text = PlacarA.ToString();
                EditA.Text = PontosA.Text;

                Is20Pontos(1);
                PlacarB = s.pontosB;
                PontosB.Text = PlacarB.ToString();
                EditB.Text = PontosB.Text;

                Is20Pontos(2);
                _20M.IsChecked = s.Is20M;
                _2X.IsChecked = s.Is2X;
                JogdoresA.PlaceholderText = s.nomeA;
                JogdoresB.PlaceholderText = s.nomeB;
            };
            d.SecondaryButtonClick += (ss, es) => {   };
            await d.ShowAsync();
            paneH.IsPaneOpen = !paneH.IsPaneOpen;
        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender as Button;
            try
            {
                db.Banco_De_Dados.Classes.Salvos s = btn.DataContext as db.Banco_De_Dados.Classes.Salvos;
                MensagemError($"{loader.GetString("Estapartidafoirealizadanodia")} {s.data} {loader.GetString("com")} {s.nomeA} {loader.GetString("vs")} {s.nomeB} {loader.GetString("e")} {s.Result} {loader.GetString("com")} {s.pontosA} {loader.GetString("a")} {s.pontosB}{System.Environment.NewLine}{System.Environment.NewLine}{x20partida} {x2partida}");


            }
            catch (Exception x) { MensagemError(x.Message); }
        }
        string x20partida, x2partida;
        void AddSalvosDB()
        {
            try
            {
                if (PlacarA != 0 || PlacarB != 0)
                {
                    if (PlacarA == PlacarB)
                    {
                        x1 = PlacarA; x2 = PlacarB;
                        n1 = NomesDosJogadores(1);
                        n2 = NomesDosJogadores(2);
                        x = NomesDosJogadores(1) + $" {loader.GetString("e")} " + NomesDosJogadores(2);
                        _result = loader.GetString("empatou");
                    }
                    else if (PlacarA > PlacarB)
                    {
                        x1 = PlacarA; x2 = PlacarB;
                        n1 = NomesDosJogadores(1);
                        n2 = NomesDosJogadores(2);
                        x = NomesDosJogadores(1);
                        _result = loader.GetString("ganhou");
                    }
                    else if (PlacarB > PlacarA)
                    {
                        x2 = PlacarA; x1 = PlacarB;
                        n1 = NomesDosJogadores(2);
                        n2 = NomesDosJogadores(1);
                        x = NomesDosJogadores(2);
                        _result = loader.GetString("ganhou");
                    }
                    if (_20M.IsChecked.Value) x20partida = loader.GetString("Partidade20pontos"); else x20partida = loader.GetString("Partidanormal");
                    if (_2X.IsChecked.Value) x2partida = loader.GetString("edobrada"); else x2partida = "";

                    string d = DateTime.Now.ToString();

                    db.Banco_De_Dados.Classes.Salvos salvos = new db.Banco_De_Dados.Classes.Salvos() { data = d, pontosA = x1, pontosB = x2, nomeA = n1, nome = x, nomeB = n2, Result = _result, pontosA1 = PlacarA, pontosB2 = PlacarB, Is20M = _20M.IsChecked.Value, Is2X = _2X.IsChecked.Value, Text1 = $"{n1} vs {n2}",
                        Text2 = $"{x20partida} {x2partida}",
                        Text3 = $"{x} {_result} de {x1} a {x2} no dia {d}"
                    };
            
                    GetSalvos.Salvar(salvos);
                }
            }
            catch(Exception x)
            {
                MensagemError(x.Message);
            }
        }

     
    }
    #endregion
}

