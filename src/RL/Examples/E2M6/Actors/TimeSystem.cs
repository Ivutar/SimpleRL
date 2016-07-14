using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E2M6.Actors
{
    class TimeSystem
    {
        public double TotalTime { get; private set; }
        public List<Actor> Actors;

        public TimeSystem ()
        {
            Actors = new List<Actor>();
        }

        public void UpdateActor(Actor actor, double energy)
        {
            //validate actor
            if (Actors == null || Actors.Count <= 0 || actor == null || !Actors.Contains(actor))
                return;

            //actor must be on top
            if (Actors.Where(a => a.Energy >= 1000).OrderByDescending(a => a.Energy).FirstOrDefault() != actor)
                return;

            UpdateEvent e = new UpdateEvent() { EnegryCost = energy, StopUpdating = false };
            actor.Energy -= e.EnegryCost;
        }

        public void UpdateActors(Actor stopper)
        {
            if (Actors == null || Actors.Count <= 0 || stopper == null || !Actors.Contains(stopper))
                return;

            while (true)
            {
                var actors = Actors.Where(a => a.Energy >= 1000).OrderByDescending(a => a.Energy);
                if (actors.Count() > 0)
                {
                    foreach (var a in actors)
                    {
                        if (a == stopper)
                            return;

                        UpdateEvent e = new UpdateEvent() { EnegryCost = 1000, StopUpdating = false };

                        a.Update(ref e);

                        a.Energy -= e.EnegryCost;
                        if (e.StopUpdating)
                            break;
                    }
                }
                else
                {
                    foreach (var a in Actors)
                        a.Energy += a.Speed;
                }
            }
        }
    }
}
