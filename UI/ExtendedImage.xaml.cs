﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MyToolkit.Phone;

namespace MyToolkit.UI
{
	public partial class ExtendedImage : UserControl
	{
		public ExtendedImage()
		{
			InitializeComponent();
		}

		#region Properties

		public static readonly DependencyProperty ShowBackgroundProperty =
			DependencyProperty.Register("ShowBackground", typeof (bool), typeof (ExtendedImage), new PropertyMetadata(true, PropertyChangedCallback));

		public bool ShowBackground
		{
			get { return (bool) GetValue(ShowBackgroundProperty); }
			set { SetValue(ShowBackgroundProperty, value); }
		}

		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register("Source", typeof(Uri), typeof(ExtendedImage), new PropertyMetadata(default(Uri), PropertyChangedCallback));

		public Uri Source
		{
			get { return (Uri) GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public static readonly DependencyProperty SourceLightProperty =
			DependencyProperty.Register("SourceLight", typeof (Uri), typeof (ExtendedImage), new PropertyMetadata(default(Uri), PropertyChangedCallback));

		public Uri SourceLight
		{
			get { return (Uri) GetValue(SourceLightProperty); }
			set { SetValue(SourceLightProperty, value); }
		}

		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof (Stretch), typeof (ExtendedImage), new PropertyMetadata(default(Stretch), PropertyChangedCallback));

		public Stretch Stretch
		{
			get { return (Stretch) GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		#endregion

		private static void PropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var ctrl = (ExtendedImage)obj;
			if (e.Property == StretchProperty)
				ctrl.Image.Stretch = ctrl.Stretch;
			else if (e.Property == ShowBackgroundProperty)
			{
				ctrl.LayoutRoot.Background = ctrl.ShowBackground ?
					(Brush)ctrl.Resources["PhoneInactiveBrush"] : new SolidColorBrush(Colors.Transparent);
			}
			else if (e.Property == SourceLightProperty || e.Property == SourceProperty)
				ctrl.UpdateSource();
		}

		private void UpdateSource()
		{
			if (PhoneApplication.IsDarkTheme || SourceLight == null)
				Performance.LowProfileImageLoader.SetUriSource(Image, Source);
			else
				Performance.LowProfileImageLoader.SetUriSource(Image, SourceLight);
		}
	}
}
