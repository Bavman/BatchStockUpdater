   M S B A M L   `   `   `  ���� O  KBatchStockUpdater.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:BatchStockUpdater.UIBatchStockUpdater.UI  %    BatchStockUpdater.UI.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:BatchStockUpdater.UI       � Title$  Batch Stock Updater��$	��800��$
��1120�� � 	Resources  �� 
TargetType �� Property �� Value��%�   '��       �� " ��.���  � ��$ 10q���  � ��$ 20(���  � ��$ Center=���  � ��$	 150����  � ��$ 40��.���  � ColumnDefinitions �� $��* ��� $	��680 ��� $��* � � RowDefinitions �� $	��67* ��� $	��515 ��� $	��60* �.+��� $��Stretch=�$��Stretch=�$��1��._��� $��Stretch=�$��Stretch=�.+��� -    ��Name$ LoginButton��$��Login��$��0��$��1���� -   $ LogOutButton��$��Logout��$��0��$��1��{�.  �� -   $ ImportCSVButton��$��
Import CSV��$��0��$��1��{�.  �� -   $ ExportCSVButton��$��
Export CSV��$��0��$��1��$��Bottom=�{�.  $��28,0,28,204q�$	��150���� -   $ ExportXMLButton1��$��Export XML Style 1��$��17(�$��0��$��1��$��Bottom=�{�.  $��28,0,28,121q�$	��150���� -   $ ExportXMLButton2��$��Export XML Style 2��$��17(�$��0��$��1��$��Bottom=�{�.  $��
28,0,28,76q�$	��150���� $��Stretch=�$��Stretch=�
���   �$��1q�$��1��$��1���� $��Stretch=�$��Stretch=�$��1��$��2��._��� $��Stretch=�$��Stretch=�.+��� -   $ PreferencesButton��$��Preferences��$��0��$��1���� -   $ ManageUsersButton��$��Manage Users��$��0��$��1���� -	   $ 
LogsButton��$
��Logs��$��0��$��1��&   System.Windows.Controls.DataGrid  -
     Name$ CSVDataGrid��$��1��$
��Left=�$	��495��$��	10,10,0,0q�$��1��$	��Top=�$	��660��$��16(��� 
���   �$��1q�$��3��$
��Left=�$��60��$��10,10,-90,0q�$	��Top=�$
��1060��._��� -   	 ��Name$	 label��$��Batch Stock Updater��$��36(��� -   $ OpenFolderButton��$��Open Folder��$��17(�$��0��$��1��$��Bottom=�{�.  $��
28,0,28,10q�$	��150��