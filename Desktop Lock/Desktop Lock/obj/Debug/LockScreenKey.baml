   M S B A M L   `   `   `  ����  Desktop Lock     Desktop_Lock.LockScreenKey   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  LockScreenKey��5   	   $
��1080��6   $
��1920��6-    � WindowStyle$
 None=�6:   
������  � WindowState$ 	Maximized=�6}    � AllowsTransparency . .��5      � -   5         �Name$	 ABC��5      $
��Left=�6   .+�5   
   �� -   5         ��Name$ 	rectangle��5      ���#000000$
��Left=�66   $	��100��6Q   $��0,0,0,0q�6^   
���   �$	��Top=�6~   $	��100��6�    J� Opacity$
 0.01Q�6�   �� 5   
   -   5         ��Name$	 img��5      $��/Image/autumn.png��6   $
��1080��68   $
��1920��6F   $ 1Q�6S   �� 5   
   -   5        $
 img1��5      $��/Image/FrontWindow .png��6   $��0,0,0,0q�6?   $	��261��6P   $	��400��6]   $ 1Q�6i   6w   �� 5	   
   -   5        $
 img2��5	      $��/Image/glock.png��6   $��
0,-110,0,0q�68   $	��120��6L   $	��120��6Y   $ 1Q�6e   6s   �� 5
   
   -   5         ��Name$ button��5
      $��确定��6!   $��1065,471,795,0q�6.   $	��Top=�6F   $��60��6^   $
��Left=�6~   �� 5   
   -   5        	 ��Name$	 slider��5      $��0,0,0,0q�6!   
 � Maximum$	
 100Q�62    � Minimum$ 30Q�6@   $	��206��6M    � Value$	 100Q�6Y   $	��Top=�6�   $
��Left=�6�   G� 5   
   -	   5         G�Name$ passwordbox��5      $��	0,473,0,0q�6+   $	��Top=�6>   $	��200��6V    G� 	MaxLength$ 16��6�    �� PreferredImeState$	 Off=�6�    �� IsInputMethodEnabled .  $
��Left=�6�   �� 5   
   -
   5         ��Name$ 
textblock1��5      $��795,499,0,0q�6(   $
��Wrap=�6=   $��解锁密码:��6Q   $	��Top=�6^   $
��Left=�6v   �� 5   
   -   5        $ 
textblock2��5      $��795,520,-182,0q�6(   $
��Wrap=�6@   $��
透明度:��6T   $	��Top=�6`   $
��Left=�6x   5      5      