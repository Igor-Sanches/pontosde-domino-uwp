﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// App Pontos de dominó programado em C# por Igor De Jesus Dutra Sanches em Picada/Anajatuba - MA // Projeto iniciado no dia 20/01/2019 para uso próprio em partidas de dominó como um placar eletrónico

namespace Pontos_de_Domino_UWP
{
    /// <summary>
    ///Fornece o comportamento específico do aplicativo para complementar a classe Application padrão.
    /// </summary>
    sealed partial class App : Application
    {
        bool isMobile = false;
        /// <summary>
        /// Inicializa o objeto singleton do aplicativo.  Esta é a primeira linha de código criado
        /// executado e, como tal, é o equivalente lógico de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }
        private async void MensagemError(string v)
        {
            var d = new ContentDialog()
            {
                Title = "Pontos de Dominó",
                Content = v,
                CloseButtonText = "Fechar",
                RequestedTheme = ElementTheme.Dark,
                Background = ((SolidColorBrush)App.Current.Resources["Background"])

            };
            d.CloseButtonClick += (s, e) => { };
            await d.ShowAsync();
        }
        /// <summary>
        /// Chamado quando o aplicativo é iniciado normalmente pelo usuário final.  Outros pontos de entrada
        /// serão usados, por exemplo, quando o aplicativo for iniciado para abrir um arquivo específico.
        /// </summary>
        /// <param name="e">Detalhes sobre a solicitação e o processo de inicialização.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            isMobile = ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar");
            if (isMobile)
            {
                var statusbar = StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = ((SolidColorBrush)App.Current.Resources["isBack"]).Color;
                statusbar.ForegroundColor = Colors.WhiteSmoke;
                statusbar.BackgroundOpacity = 100;
                await statusbar.HideAsync();
            }
            else
            {
                var view = ApplicationView.GetForCurrentView().TitleBar;
                view.BackgroundColor = ((SolidColorBrush)App.Current.Resources["isBack"]).Color;
                view.ForegroundColor = Colors.White;
                view.ButtonBackgroundColor = ((SolidColorBrush)App.Current.Resources["isBack"]).Color; 
                view.ButtonForegroundColor = Colors.White;
            }
            // Não repita a inicialização do aplicativo quando a Janela já tiver conteúdo,
            // apenas verifique se a janela está ativa
            if (rootFrame == null)
            {
                // Crie um Quadro para atuar como o contexto de navegação e navegue para a primeira página
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Carregue o estado do aplicativo suspenso anteriormente
                }

                // Coloque o quadro na Janela atual
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Quando a pilha de navegação não for restaurada, navegar para a primeira página,
                    // configurando a nova página passando as informações necessárias como um parâmetro
                    // parâmetro
                    try
                    {
                        rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    }
                    catch (Exception x) { MensagemError(x.Message); }
                }
                // Verifique se a janela atual está ativa
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Chamado quando ocorre uma falha na Navegação para uma determinada página
        /// </summary>
        /// <param name="sender">O Quadro com navegação com falha</param>
        /// <param name="e">Detalhes sobre a falha na navegação</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Chamado quando a execução do aplicativo está sendo suspensa.  O estado do aplicativo é salvo
        /// sem saber se o aplicativo será encerrado ou retomado com o conteúdo
        /// da memória ainda intacto.
        /// </summary>
        /// <param name="sender">A fonte da solicitação de suspensão.</param>
        /// <param name="e">Detalhes sobre a solicitação de suspensão.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Salvar o estado do aplicativo e parar qualquer atividade em segundo plano
            deferral.Complete();
        }
    }
}
