   M S B A M L   `   `   `  ����  Project_sfpgu_desktop.    (Project_sfpgu_desktop.Views.SchedulePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � 	Resources  5         BoolToVisibilityConverter �� 
TargetType �� Property �� Value��%�   &	        '��      �� 5	   
   6'   �� 5   
   " ��6   .�5      ��  � ��$ 14(�6)   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 0 10 0 0q�6'   �� 5       � ��$ 10q�6(   5      5      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��18(�5      $
��Bold"�6"   $��Center=�5      $��0 0 0 10q�5       ��5      �� *	GroupName+5       �� 5#   
    �� Orientation$ 
Horizontal=�6   $��1��6.   $
��Left=�6;   .+�5$      �� $	��200��6   $��30��6"   $��Center=�5&      $��0 0 10 0q�5'      ��5%      �� *	GroupName+ �� UpdateSourceTrigger$ PropertyChanged=�5'      �� 5(      -   5        $��Загрузить��5(      $��30��6)   5)      5*      �� 5-   
   $ 
Horizontal=�6   $��1��6.   $��Right=�6;   .+�5.      �� $��Добавить��6   $	��5,0q�6(   $
��10,5q�65   ��5/      �� *AddScheduleCommand+y�50      �� *
IsEditable+ �� 	Converter# [  �� 51      $ ��Редактировать��6   $	��5,0q�6-   $
��10,5q�6:   ��52      �� *EditScheduleCommand+y�53      �� *
IsEditable+# [  �� 54      $��Удалить��6   $	��5,0q�6'   $
��10,5q�64   ��55      �� *DeleteScheduleCommand+y�56      �� *
IsEditable+# [  57      �� 5:   
   $��Расписание��6   $��2��6'   .��5;      &   System.Windows.Controls.DataGrid     AutoGenerateColumns .  	   
IsReadOnly	 . $��0q�5>      
   HeadersVisibility$
 Column=�6      SelectionMode$ Single=�5?         SelectionUnit$ FullRow=�6"   ��5;      �� *
Schedule+ �� SelectedItem 5<      �� *SelectedScheduleItem+ �� Mode$ TwoWay=�   Columns 5A      0  *System.Windows.Controls.DataGridTextColumn  5B      ,  &System.Windows.Controls.DataGridColumn   Header$ Дата��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6g   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Date+ �� StringFormat$ dd/MM/yy��6g     5C      $ 
Время��6)   $ * 6Q    68   �� *Time+6Q     5D      $ Предмет��6)   $ 2* 6V    6:   �� *	Subject+6V     5E      $  Преподаватель��6)   $ 2* 6d    6@   �� *TeacherFullName+6d   5G      5H      �� 5J   
   $��Посещаемость��6   $��3��6)   .��5K         .     CanUserAddRows .  $
 Column=�5L      $ Single=�6*   $ FullRow=�6A   ��5K      �� *
Attendance+	 5M      �� *
IsEditable+# [   �� ConverterParameter$ False�� 5N        5O      $ Студент��6)   $ 2* 6^    6:   �� *StudentFullName+6^     5P      $ Дата��6)   $ * 6g    67   �� *Date+$ dd/MM/yy��6g   4  .System.Windows.Controls.DataGridCheckBoxColumn  5Q      $ Присутствует��6-   $ * 6�    6C   �� *	IsPresent+$ TwoWay=�$ PropertyChanged=�6�   5S      5T      5U      5V      