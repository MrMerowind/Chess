   M S B A M L   `   `   `  ����F  BChessEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ChessEngineChessEngine  5   	       ChessEngine.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ChessEngine     5   	   -   5          �Name$  MainChessWindow��5   	    � Title$ Szachy by MrMerowind��5   	   $	��600��6&   $	��746��63   $	��746��6?   $	��600��6N   $	��746��6^   $	��600��6m   .��5	      � ���	#FF176E6E � ColumnDefinitions 5
   
   �� 5      $
��160* �6   �� 5      $
��304* �6   �� 5      $
��275* �6   .+�5   
   �� -   5         ��Name$ Board��5      $
��Left=�6    $	��496��6;   $��	10,36,0,0q�6H   $	��Top=�6[   $	��496��6s   ���	#FF939393$��3��6�   5      �� 5   
   -   5         ��Name$ 
BlackTimer��5      $��Czarne: 00:00��6$   $
��Left=�6<   $��	10,10,0,0q�6W   $	��Top=�6j    J� RenderTransformOrigin$ 0.29,-0.004&�6�   $	��221��6�   
���   �
�������$��Center=�6�   $��Center=�6�   $��2��6  �� 5   
   -   5        $ 
WhiteTimer��5      $��Białe: 00:00��6$   $
��Left=�6;   $��
10,533,0,0q�6V   $	��Top=�6j   $	��221��6�   
�������$��Center=�6�   $��Center=�6�   $��2��6�   �� 5   
   -   5         ��Name$ ButtonPlayWithPerson��5      $��Graj z osobą��6/   $
��Left=�6F   $��	75,36,0,0q�6a   $	��Top=�6t   $	��179��6�   $��28��6�   $��2��6�   6�   �� 5   
   -   5        $# ButtonPlayWithComputerAsWhite��5      $ ��Graj z komputerem białymi��68   $
��Left=�6\   $��	75,69,0,0q�6w   $	��Top=�6�   $	��179��6�   $��28��6�   $��2��6�   6�   �� 5   
   -   5        $# ButtonPlayWithComputerAsBlack��5      $ ��Graj z komputerem czarnymi��68   $
��Left=�6]   $��
75,102,0,0q�6x   $	��Top=�6�   $	��179��6�   $��28��6�   $��2��6�   6�   �� 5   
   -   5        $ ButtonPlayTwoComputers��5      $��Gra dwóch komputerów��61   $
��Left=�6P   $��
75,135,0,0q�6k   $	��Top=�6   $	��179��6�   $��28��6�   $��2��6�   6�   5      5      