   M S B A M L   `   `   `  ����B  >Control, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ControlControl  5   	       Control.ViewKeyLogWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Control     5   	     � Title$  VIEW KEY LOG��5   	   $	��300��6   $��442.416��6+   .��5	      � $��10,10,10,10q�6    � RowDefinitions 5
   
   �� 5      $
��auto �6   �� 5      .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      �� -   5        $��lblSessionID��5      $��Session ID (MAX = ): ��6,   $��Center=�6I   �� 5      -   5        $��txtSessionid��5      $��30��6+   �� 5      -   5        $��btnView��5      $
��VIEW��6$   $��10,0,0,0q�63   6E   5      �� 5   
   -   5        $��txtLog��5      $��0,10,0,0q�6$   $��1��66    �� 
IsReadOnly . 6C   5      5      