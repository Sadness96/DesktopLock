   M S B A M L   `   `   `  ����  Desktop Lock    Desktop_Lock.SetUp   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  SetUp��5   	   $	��300��6   $	��500��6$    � AllowsTransparency .  � WindowStyle$
 None=�6J   
������ .��5      � .+�5   
   �� -   5         ��Name$	 img��5      $��/Image/back.png��6   $��0,0,0,0q�66   $	��300��6G   $	��500��6T    J� Opacity$
 0.95Q�6`   6�   �� 5   
   -   5        $
 img1��5      $��/Image/confirm.png��6   $��280,248,140,28q�6:   $��25��6R   $��80��6^   6�   �� 5   
   -   5        $
 img2��5      $��/Image/cancel.png��6   $��390,248,30,27q�69   $��25��6P   $��80��6\   6�   �� 5	   
   $��265,25,-72,108q�6   .+�5
      	� -   5         	�Name$ radio1��5
       	� 	GroupName$ x��6*   $�� 密码解锁��68   $��26(�6H   
�������$��0,0,0,0q�6i   6z   	� 5      -   5        $ radio2��5      $ x��6*   $�� 时间解锁��68   $��26(�6H   
�������$��0,0,0,0q�6i   6z   	� 5      -   5        $ radio3��5      $ x��6*   $�� U 盘解锁��68   $��26(�6I   
�������$��0,0,0,0q�6j   6{   5      �� 5   
   -   5        $
 img3��5      $��/Image/password.png��6   $��115,46,322,191q�6;   �� 5   
   -	   5        $
 img4��5      $��/Image/greybars.png��6   $��33,140,30,159q�6;   $��1��6R   $	��437��6]   6k   �� 5   
   -
   5         ��Name$ libel1��5      $%��使用上次锁屏密码解锁:��6    $
��Left=�66   $��
53,153,0,0q�6Q   $	��Top=�6e   
�������$��20(�6�   �� 5   
   -   5        $ libel2��5      $%��非正常关闭开机自启动:��6    $
��Left=�66   $��
53,193,0,0q�6Q   $	��Top=�6e   
�������$��20(�6�   �� 5   
   -   5        $
 img5��5      $��/Image/off.png��6   $��343,156,87,113q�66   6k   �� 5   
   -   5        $
 img6��5      $��/Image/off.png��6   $��343,197,87,72q�66   6j   5      5      