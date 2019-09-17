﻿using System;

namespace dataset
{
    public class ColumnChangedEventArgs : EventArgs
    {
        private int type;
        string oldName;
        string newName;

        IColumn column;

        public ColumnChangedEventArgs(int type)
        {
            this.type = type;
        }

        //changed name from avadis java method since GetType is already existing method in c# object.
        public int GetChangeType()
        {
            return type;
        }

        public IColumn GetColumn()
        {
            return Column;
        }

       

        public string GetOldName()
        {
            return OldName;
        }

        public string GetNewName()
        {
            return NewName;
        }

        //XXX-Anand: HACK
        // hack to cancel the setName on column
        bool cancelled;

        public string OldName { get => oldName; set => oldName = value; }
        public string NewName { get => newName; set => newName = value; }
        public IColumn Column { get => column; set => column = value; }
        public bool Cancelled { get => cancelled; set => cancelled = value; }

        public void Cancel()
        {
            Cancelled = true;
        }
    }
    public delegate void ColumnChangedEventHandler(object sender, ColumnChangedEventArgs e);
}
