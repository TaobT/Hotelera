// Copyright (c) 2015 - 2023 Doozy Entertainment. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

//.........................
//.....Generated Class.....
//.........................
//.......Do not edit.......
//.........................

using System.Collections.Generic;
// ReSharper disable All
namespace Doozy.Runtime.UIManager.Containers
{
    public partial class UIView
    {
        public static IEnumerable<UIView> GetViews(UIViewId.HoteleraScene id) => GetViews(nameof(UIViewId.HoteleraScene), id.ToString());
        public static void Show(UIViewId.HoteleraScene id, bool instant = false) => Show(nameof(UIViewId.HoteleraScene), id.ToString(), instant);
        public static void Hide(UIViewId.HoteleraScene id, bool instant = false) => Hide(nameof(UIViewId.HoteleraScene), id.ToString(), instant);

        public static IEnumerable<UIView> GetViews(UIViewId.LoginRegister id) => GetViews(nameof(UIViewId.LoginRegister), id.ToString());
        public static void Show(UIViewId.LoginRegister id, bool instant = false) => Show(nameof(UIViewId.LoginRegister), id.ToString(), instant);
        public static void Hide(UIViewId.LoginRegister id, bool instant = false) => Hide(nameof(UIViewId.LoginRegister), id.ToString(), instant);
    }
}

namespace Doozy.Runtime.UIManager
{
    public partial class UIViewId
    {
        public enum HoteleraScene
        {
            ConfirmarBorrado,
            DetallesHotel,
            FormHoteles,
            ListaHoteles
        }

        public enum LoginRegister
        {
            Login,
            Register
        }    
    }
}
