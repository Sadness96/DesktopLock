﻿#pragma checksum "..\..\Skin.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DC241D3549F58DC9B6F35C698A1873D7"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18408
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Desktop_Lock {
    
    
    /// <summary>
    /// Skin
    /// </summary>
    public partial class Skin : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image img;
        
        #line default
        #line hidden
        
        
        #line 7 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image img1;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image img2;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radio1;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radio2;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radio3;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton radio4;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label libel;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textbox1;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\Skin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button button;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Desktop Lock;component/skin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Skin.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\Skin.xaml"
            ((Desktop_Lock.Skin)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.img = ((System.Windows.Controls.Image)(target));
            
            #line 6 "..\..\Skin.xaml"
            this.img.MouseMove += new System.Windows.Input.MouseEventHandler(this.img_MouseMove);
            
            #line default
            #line hidden
            return;
            case 3:
            this.img1 = ((System.Windows.Controls.Image)(target));
            
            #line 7 "..\..\Skin.xaml"
            this.img1.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.img1_MouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.img2 = ((System.Windows.Controls.Image)(target));
            
            #line 8 "..\..\Skin.xaml"
            this.img2.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.img2_MouseDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.radio1 = ((System.Windows.Controls.RadioButton)(target));
            
            #line 10 "..\..\Skin.xaml"
            this.radio1.Checked += new System.Windows.RoutedEventHandler(this.radio1_Checked);
            
            #line default
            #line hidden
            return;
            case 6:
            this.radio2 = ((System.Windows.Controls.RadioButton)(target));
            
            #line 11 "..\..\Skin.xaml"
            this.radio2.Checked += new System.Windows.RoutedEventHandler(this.radio2_Checked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.radio3 = ((System.Windows.Controls.RadioButton)(target));
            
            #line 12 "..\..\Skin.xaml"
            this.radio3.Checked += new System.Windows.RoutedEventHandler(this.radio3_Checked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.radio4 = ((System.Windows.Controls.RadioButton)(target));
            
            #line 13 "..\..\Skin.xaml"
            this.radio4.Checked += new System.Windows.RoutedEventHandler(this.radio4_Checked);
            
            #line default
            #line hidden
            return;
            case 9:
            this.libel = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.textbox1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.button = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\Skin.xaml"
            this.button.Click += new System.Windows.RoutedEventHandler(this.button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
