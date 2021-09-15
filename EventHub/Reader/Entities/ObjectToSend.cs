using System;
using System.Text.Json;

namespace Reader.Entities
{
    public class ObjectToSend
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public ObjectToSend(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
