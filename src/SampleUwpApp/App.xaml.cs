﻿using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MyToolkit.Controls;
using MyToolkit.Paging;
using SampleUwpApp.Views;
using AppBarButton = Windows.UI.Xaml.Controls.AppBarButton;

namespace SampleUwpApp
{
    sealed partial class App : MtApplication
    {
        private HamburgerFrameBuilder _hamburgerFrameBuilder;

        public App()
        {
            InitializeComponent();
        }

        public override Type StartPageType => typeof(MainPage);

        public override UIElement CreateWindowContentElement()
        {
            _hamburgerFrameBuilder = new HamburgerFrameBuilder();

            var searchItem = new SearchHamburgerItem
            {
                PlaceholderText = "Search",
            };
            searchItem.QuerySubmitted += async (sender, args) =>
            {
                await _hamburgerFrameBuilder.MoveOrNavigateToPageAsync(typeof (DataGridPage));
                var dataGridPage = (DataGridPage) _hamburgerFrameBuilder.Frame.CurrentPage.Page;
                dataGridPage.Model.Filter = args.QueryText;
            };
            
            _hamburgerFrameBuilder.Hamburger.Header = new HamburgerHeader();
            _hamburgerFrameBuilder.Hamburger.TopItems = new ObservableCollection<HamburgerItem>
            {
                new PageHamburgerItem
                {
                    Content = "Home",
                    ContentIcon = new SymbolIcon(Symbol.Home),
                    Icon = new SymbolIcon(Symbol.Home),
                    PageType = typeof(MainPage)
                },
                searchItem,
                new PageHamburgerItem
                {
                    Content = "Movie",
                    ContentIcon = new SymbolIcon(Symbol.Video),
                    Icon = new SymbolIcon(Symbol.Video),
                    PageType = typeof(MoviePage)
                },
                new PageHamburgerItem
                {
                    Content = "Article",
                    ContentIcon = new SymbolIcon(Symbol.PreviewLink),
                    Icon = new SymbolIcon(Symbol.PreviewLink),
                    PageType = typeof(ArticlePage)
                }, 
                new PageHamburgerItem
                {
                    Content = "DataGrid",
                    ContentIcon = new SymbolIcon(Symbol.ViewAll),
                    Icon = new SymbolIcon(Symbol.ViewAll),
                    PageType = typeof(DataGridPage)
                }
            };
            _hamburgerFrameBuilder.Hamburger.BottomItems = new ObservableCollection<HamburgerItem>
            {
                new PageHamburgerItem
                {
                    Content = "Settings",
                    ContentIcon = new SymbolIcon(Symbol.Setting),
                    Icon = new SymbolIcon(Symbol.Setting),
                    PageType = typeof(SettingsPage)
                }
            };
            return _hamburgerFrameBuilder.Hamburger;
        }

        public override MtFrame GetFrame(UIElement windowContentElement)
        {
            return _hamburgerFrameBuilder.Frame;
        }
    }
}
