   M S B A M L   `   `   `  ����B  >Control, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ControlControl  5   	   $    Control.UploadNewVersionWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Control     5   	     � Title$  Upload New Version��5   	   $	��300��6$   $	��300��61   .��5	      � $��10,10,10,10q�6    � RowDefinitions 5
   
   �� 5      $
��auto �6   �� 5      �� 5      $
��auto �6   .+�5   
   ]� .+�5      �� -   5        $��
lblVersion��5      $��	Version: ��6*   $��Center=�6;   �� 5      -   5        $��btnSelectFile��5      $��Select File��6*   $��Right=�6@   $	��100��6\   $��25��6h   6t   5      �� 5   
   $��1��6   $��	0,10,0,10q�6   $��1q�61   
���   �._�5      �� -   5        $��lblPath��5   %   $��1��6   6%   5      ]� 5   
   $��2��6   .+�5      �� -   5        $��	btnUpload��5      $��Upload��6&   $
��Left=�67   $	��100��6R   $��30��6^   6j   �� 5      -   5        $��	btnCancel��5      $��Cancel��6&   $��Right=�67   $	��100��6S   6_   5      5      5       