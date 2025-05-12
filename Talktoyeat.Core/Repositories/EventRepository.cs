// using System;
// using System.Collections.Generic;
// using System.Linq;

// namespace Talktoyeat.Core.Repositories
// {
//     public class EventRepository
//     {
//         private readonly List<Event> _events;

//         public EventRepository()
//         {
//             _events = new List<Event>();
//         }

//         public void AddEvent(Event newEvent)
//         {
//             if (newEvent == null)
//                 throw new ArgumentNullException(nameof(newEvent));

//             _events.Add(newEvent);
//         }

//         public IEnumerable<Event> GetAllEvents()
//         {
//             return _events.AsReadOnly();
//         }

//         public Event GetEventById(Guid eventId)
//         {
//             return _events.FirstOrDefault(e => e.Id == eventId);
//         }

//         public bool DeleteEvent(Guid eventId)
//         {
//             var eventToRemove = _events.FirstOrDefault(e => e.Id == eventId);
//             if (eventToRemove != null)
//             {
//                 _events.Remove(eventToRemove);
//                 return true;
//             }
//             return false;
//         }
//     }

//     public class Event
//     {
//         public Guid Id { get; set; }
//         public string Name { get; set; } = string.Empty; // Initialize to avoid nullability warnings
//         public DateTime Date { get; set; }
//         // ...other properties...
//     }
// }
