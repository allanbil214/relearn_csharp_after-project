   M S B A M L   `   `   `  ����E  ASimpleToDo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SimpleToDo
SimpleToDo  5   	   !    SimpleToDo.MotivationWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:SimpleToDo     5   	     � Title$  MotivationWindow��5   	   $	��406��6"   $	��277��6/    � 
ResizeMode$ CanMinimize=�6;   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ mainMotivLabel��5      $��Motivating Motivation��5      $
��Bold"�5      $��14(�5      $��Segoe UI Semibold)�5      $
��Left=�5      $��0,0,0,5q�5      &   System.Windows.Controls.DataGrid  5!   
   -   5          Name$ 
motivTable��5!      $��1��5"         AutoGenerateColumns$
 True��5#      $��	0,10,0,10q�5%         
IsReadOnly$
 True��5&         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6%      SelectionMode$ Single=�68   6O   �� 5)   
   -   5        $ 
motivLabel��5)      $��2��5*      $��Motivation Text��5+      $��Segoe UI Semibold)�5,      $��0,0,0,3q�5-      �� 50   
   -   5        	 ��Name$	 motivationTextBox��50      $��3��51      
 �� TextWrapping$

 Wrap=�52      $��32��53      $��Center=�54      $��Stretch=�55      �� 58   
   -   5         ��Name$ 
saveButton��58      $��4��59      $��Save It��5:      $
��Bold"�5;      $��39��5<      $��
128,10,1,0q�5=      5>      �� 5?   
   -   5        $ clearButton��5?      $��4��5@      $��Clear It��5A      $
��Bold"�5B      $��39��5C      $��
0,10,129,0q�5D      5E      5F      5G      