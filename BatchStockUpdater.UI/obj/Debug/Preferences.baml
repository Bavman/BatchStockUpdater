   M S B A M L   `   `   `  ����O  KBatchStockUpdater.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:BatchStockUpdater.UIBatchStockUpdater.UI  5   	   ,    &BatchStockUpdater.UI.PreferencesWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:BatchStockUpdater.UI     5   	     � Title$  Preferences��5   	   $	��330��6   $	��600��6*   .��5	      � .+�5
   
   �� 
���   �$��1q�6%   $
��Left=�69   $	��121��6T   $��	61,42,0,0q�6a   $	��Top=�6t   $	��471��6�   �� 5   
   -   5         ��Name$ EnableDateTimeCheckBox��5      $+��%Check CSV date and time before import��63   $
��Left=�6c   $��	87,53,0,0q�6~   $	��Top=�6�   $��14(�6�    d� 	IsChecked$
 TrueY�6	  �� 5   
   -   5         ��Name$ TimeTextBlock��5      $
��Left=�6+   $��
131,90,0,0q�6F   $
��Wrap=�6Z   $	��Top=�6n   $��14(�6�   �� 6�   $#�Permit CSV import after time ��6�   �� 6�   $�
(eg 15:00)��6�   6�   �� 5   
   -   5         ��Name$ TimeTextBox��5      $
��Left=�6'   $��24��6B   $��
391,90,0,0q�6N    �� TextWrapping$
 Wrap=�6b   $�� ��6v   $	��Top=�6~   $	��100��6�   6�   �� 5   
   -   5        $ DaysBeforeTextBlock��5      $
��Left=�61   $��131,124,0,0q�6L   $
��Wrap=�6a   $	��Top=�6u   $��14(�6�   $&�� Allow CSV Import 'x' days before��6�   �� 5   
   -   5        $ DaysBeforeTextBox��5      $
��Left=�6-   $��24��6H   $��391,118,0,0q�6T   $
 Wrap=�6i   $�� ��6}   $	��Top=�6�   $	��100��6�   6�   �� 5   
   -   5         ��Name$ FolderButton��5      $��Import Folder��6'   $
��Left=�6?   $��
87,208,0,0q�6Z   $	��Top=�6n   $	��135��6�   $��14(�6�   6�   �� 5   
   -   5         ��Name$ FolderLabel��5      $��Folder: ��6%   $
��Left=�68   $��255,207,0,0q�6S   $	��Top=�6h   $	��277��6�   
������� J� Opacity$	 0.6Q�6�   �� 5   
   -   5        $ SavePrefsButton��5      $��Save Preferences��6*   $
��Left=�6E   $��391,238,0,0q�6`   $	��Top=�6u   $	��135��6�   $��30��6�   $��14(�6�   6�   5      5      