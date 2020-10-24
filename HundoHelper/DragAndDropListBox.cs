using HundoHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace HundoHelper
{
    public delegate void OnListBoxItemMoved();
    public class DragAndDropListBox<T> : ListBox
      where T : class
    {
        private Point _dragStartPoint;

        public event OnListBoxItemMoved OnItemMoved;

        private P FindVisualParent<P>(DependencyObject child)
            where P : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            P parent = parentObject as P;
            if (parent != null)
                return parent;

            return FindVisualParent<P>(parentObject);
        }

        public DragAndDropListBox()
        {
            this.PreviewMouseMove += ListBox_PreviewMouseMove;

            var style = new Style(typeof(ListBoxItem));

            style.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));

            style.Setters.Add(
                new EventSetter(
                    ListBoxItem.PreviewMouseLeftButtonDownEvent,
                    new MouseButtonEventHandler(ListBoxItem_PreviewMouseLeftButtonDown)));

            style.Setters.Add(
                    new EventSetter(
                        ListBoxItem.DropEvent,
                        new DragEventHandler(ListBoxItem_Drop)));

            this.ItemContainerStyle = style;
        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(null);
            Vector diff = _dragStartPoint - point;
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var lbi = FindVisualParent<ListBoxItem>(((DependencyObject)e.OriginalSource));
                if (lbi != null)
                {
                    DragDrop.DoDragDrop(lbi, lbi.DataContext, DragDropEffects.Move);
                }
            }
        }

        private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

        private void ListBoxItem_Drop(object sender, DragEventArgs e)
        {
            if (sender is ListBoxItem)
            {
                var source = e.Data.GetData(typeof(T)) as T;
                var target = ((ListBoxItem)(sender)).DataContext as T;

                int sourceIndex = this.Items.IndexOf(source);
                int targetIndex = this.Items.IndexOf(target);

                Move(source, sourceIndex, targetIndex);
            }
        }

        private void Move(T source, int sourceIndex, int targetIndex)
        {
            if (sourceIndex < targetIndex)
            {
                var items = this.DataContext as IList<T>;
                if (items != null)
                {
                    Debug.WriteLine($"before: {((ICollectible)items[sourceIndex]).Name} : {((ICollectible)items[sourceIndex]).OrderIndex}");
                    items.Insert(targetIndex + 1, source);
                    items.RemoveAt(sourceIndex);
                    ((ICollectible)items[targetIndex]).OrderIndex = targetIndex;
                    Debug.WriteLine($"after: {((ICollectible)items[targetIndex]).Name} : {((ICollectible)items[targetIndex]).OrderIndex}");
                    OnItemMoved.Invoke();
                }
            }
            else
            {
                var items = this.DataContext as IList<T>;
                if (items != null)
                {
                    int removeIndex = sourceIndex + 1;
                    if (items.Count + 1 > removeIndex)
                    {
                        items.Insert(targetIndex, source);
                        items.RemoveAt(removeIndex);
                        ((ICollectible)items[targetIndex]).OrderIndex = targetIndex;
                        Debug.WriteLine($"after: {((ICollectible)items[targetIndex]).Name} : {((ICollectible)items[targetIndex]).OrderIndex}");
                        OnItemMoved.Invoke();
                    }
                }
            }
        }
    }

    public class PackagesDragAndDropListBox : DragAndDropListBox<HiddenPackage> { }
    public class UsjsDragAndDropListBox : DragAndDropListBox<UniqueStuntJump> { }
    public class RobberiesDragAndDropListBox : DragAndDropListBox<Robbery> { }


}
