using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssembliesAndMemoryMgmt
{
    class ContactManager : IDisposable
    {
        private int contactsAdded;
        private ContactRepository _repository;

        public ContactManager(ContactRepository repository)
        {
            _repository = repository;
            _repository.ContactAdded += HandleContactAdded;
        }

        public void Add(Contact contact)
        {
            _repository.Add(contact);
        }

        public void Dispose()
        {
        }

        public int Count => contactsAdded;

        private void HandleContactAdded(object sender, ContactEventArgs e)
        {
            Interlocked.Increment(ref contactsAdded);
        }
    }
}
