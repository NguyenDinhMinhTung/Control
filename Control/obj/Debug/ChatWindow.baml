   M S B A M L   `   `   `  ����B  >Control, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ControlControl  5   	       Control.ChatWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Control     5   	   -   5          � Title$  CHAT BOX��5   	   $	��441��6   $	��357��6'    � ShowInTaskbar .  .��5	      �  � RowDefinitions 5
   
   �� 5      $��4* �6   6*   �� 5      $
��auto �6   6,   �� 5      $��* �6   6)   .+�5   
   �� -   5        $��
txtChatBox��5      $��0��6(   $��10,10,10,10q�65    �� 
IsReadOnly . 6^   �� 5   
   -   5        $��
txtMessage��5      $��1��6$   $��10,10,10,10q�61   .��6c   ��5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   -   �� 6   6sys&clr-namespace:System;assembly=mscorlib     6-    �� 
TargetType" ��6    �� 	Resources 5         CueBannerBrush j� 
AlignmentX j� 
AlignmentY j� Stretch	 !� Visual��%�   &	        !� 5      $
 Left=�6=   $ Center=�6O   $
 None=�6c   	 5      �� 5   "   $��
Message...��6(   
�������6=   5      5      
 �� Triggers
 5      P� 5       P� Property � �� �� Empty P� Value# Z 62   . �5      ��  �� Property � �� �� Value# [  6;   5       P� 5!       � �� 62   Z� . �5"      ��  � ��# [  6;   5#      P� 5$       �@J�IsKeyboardFocused . . �5%      ��  � ��
 �����6;   5&      5(      .��5)      5*      �� 5+   
   -   5        $��btnSend��5+      $��2��6    $
��SEND��6-   $��10,10,10,10q�6<   6i   5,      5-      