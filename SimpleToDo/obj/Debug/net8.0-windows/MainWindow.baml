   M S B A M L   `   `   `  ����E  ASimpleToDo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SimpleToDo
SimpleToDo  5   	       SimpleToDo.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:SimpleToDo     5   	   -   5          �Name$  
mainWindow��5   	    � Title$ 
MainWindow��5   	   $	��450��6   $	��800��6)    � 
ResizeMode$ CanMinimize=�65   .��5	      � -   5         �Name$ mainGrid��5	      .+�5
   
   �� -   5         ��Name$ 
titleLabel��5
      $��Renna's ��6$   $
��Left=�67   $	��Top=�6R   $
��Bold"�6j   $��18(�6|   $��10,1,0,0q�6�   �� 5   
   -   5        $ subtitleLabel��5      $��To Do List App��6'   $
��Left=�6@   $��	24,23,0,0q�6[   $	��Top=�6n   ���	#FF5454546�   �� 5   
   -   5        $ motivationLabel��5      $��[motivational text]��6)   $��Right=�6G   $��	0,13,10,0q�6c   $	��Top=�6v   �� 5   
   -   5         ��Name$ 
separator1��5      $��Center=�6(   $��0,49,0,0q�6E   $	��Top=�6W   $��7��6o    J� RenderTransformOrigin$ 0.5,0.5&�6z   $	��800��6�   z�5      V� 5      .~�5      ��  �� ScaleY$ -1Q�6%   �� 5      �� 5      T� 5      5      5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ 	todoTable��5      $��
270,52,0,0q�6Q   �� 5   
   -   5        $ addLabel��5      $��Add New To-Do List��6"   $
��Left=�6?   $��6,56,0,0q�6Z   $	��Top=�6l   $
��Bold"�6�   $��14(�6�   $��Segoe UI Semibold)�6�   �� 5   
   -	   5        	 ��Name$	 addGridBorder��5      
���   �$��1q�6<   $��10,90,538,64q�6P   � 5   
   -
   5        $ addGrid��5      $��10,90,538,64q�6    .+�5      �� -   5        $ taskNameLabel��5      $��What’s the Task?��6+   $
��Left=�6F   $��10,6,0,0q�6a   $	��Top=�6s   
�������
���   �$��Segoe UI Semibold)�6�   �� 5      -   5        
 ��Name$
 taskTextbox��5      $��Center=�6+   $��0,30,0,0q�6H    �� TextWrapping$
 Wrap=�6Z   $	��Top=�6n   $	��232��6�   $��32��6�    �� TextAlignment$ Justify=�6�   $��Center=�6�   �� 5      -   5        $ taskDescLabel��5      $��Elaborate the Plan��6+   $
��Left=�6H   $��	10,68,0,0q�6c   $	��Top=�6v   
�������
���   �$��Segoe UI Semibold)�6�   �� 5      -   5        $
 taskDescTextbox��5      $��Center=�6/   $��0,92,0,0q�6L   $
 Wrap=�6^   $	��Top=�6r   $	��232��6�   $��85��6�   $ Justify=�6�   $��Center=�6�   5      5!      5"      