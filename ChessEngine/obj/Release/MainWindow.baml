   M S B A M L   `   `   `  ���� F  BChessEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ChessEngineChessEngine      ChessEngine.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:ChessEngine     -     �Name$  MainChessWindow�� � Title$ Szachy by MrMerowind��$	��600��$	��746��$	��746��$	��600��$	��746��$	��600��.��� ���	#FF176E6E � ColumnDefinitions �� $
��160* ��� $
��304* ��� $
��275* �.+��� -    ��Name$ Board��$
��Left=�$	��496��$��	10,36,0,0q�$	��Top=�$	��496�����	#FF939393$��3���� -    ��Name$ 
BlackTimer��$��Czarne: 00:00��$
��Left=�$��	10,10,0,0q�$	��Top=� J� RenderTransformOrigin$ 0.29,-0.004&�$	��221��
���   �
�������$��Center=�$��Center=�$��2���� -   $ 
WhiteTimer��$��Białe: 00:00��$
��Left=�$��
10,533,0,0q�$	��Top=�$	��221��
�������$��Center=�$��Center=�$��2���� -    ��Name$ ButtonPlayWithPerson��$��Graj z osobą��$
��Left=�$��	75,36,0,0q�$	��Top=�$	��179��$��28��$��2���� -   $# ButtonPlayWithComputerAsWhite��$ ��Graj z komputerem białymi��$
��Left=�$��	75,69,0,0q�$	��Top=�$	��179��$��28��$��2���� -   $# ButtonPlayWithComputerAsBlack��$ ��Graj z komputerem czarnymi��$
��Left=�$��
75,102,0,0q�$	��Top=�$	��179��$��28��$��2���� -   $ ButtonPlayTwoComputers��$��Gra dwóch komputerów��$
��Left=�$��
75,135,0,0q�$	��Top=�$	��179��$��28��$��2��