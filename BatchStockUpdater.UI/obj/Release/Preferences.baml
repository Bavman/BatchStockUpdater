   M S B A M L   `   `   `  ���� O  KBatchStockUpdater.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:BatchStockUpdater.UIBatchStockUpdater.UI  ,    &BatchStockUpdater.UI.PreferencesWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:BatchStockUpdater.UI       � Title$  Preferences��$	��330��$	��600��.��� .+��� 
���   �$��1q�$
��Left=�$	��121��$��	61,42,0,0q�$	��Top=�$	��471���� -    ��Name$ EnableDateTimeCheckBox��$+��%Check CSV date and time before import��$
��Left=�$��	87,53,0,0q�$	��Top=�$��14(� d� 	IsChecked$
 TrueY��� -    ��Name$ TimeTextBlock��$
��Left=�$��
131,90,0,0q�$
��Wrap=�$	��Top=�$��14(��� $#�Permit CSV import after time ���� $�
(eg 15:00)���� -    ��Name$ TimeTextBox��$
��Left=�$��24��$��
391,90,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��100���� -   $ DaysBeforeTextBlock��$
��Left=�$��131,124,0,0q�$
��Wrap=�$	��Top=�$��14(�$&�� Allow CSV Import 'x' days before���� -   $ DaysBeforeTextBox��$
��Left=�$��24��$��391,118,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��100���� -    ��Name$ FolderButton��$��Import Folder��$
��Left=�$��
87,208,0,0q�$	��Top=�$	��135��$��14(��� -    ��Name$ FolderLabel��$��Folder: ��$
��Left=�$��255,207,0,0q�$	��Top=�$	��277��
������� J� Opacity$	 0.6Q��� -   $ SavePrefsButton��$��Save Preferences��$
��Left=�$��391,238,0,0q�$	��Top=�$	��135��$��30��$��14(�