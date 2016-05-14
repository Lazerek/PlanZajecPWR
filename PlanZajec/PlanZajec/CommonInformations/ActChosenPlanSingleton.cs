﻿using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.CommonInformations
{
    class ActChosenPlanSingleton
    {
        private static ActChosenPlanSingleton instance = new ActChosenPlanSingleton();

        private ActChosenPlanSingleton() { }



        public static ActChosenPlanSingleton Instance
        {
            get
            {
                return instance;
            }
        }

        public long IdPlanu { get; private set; }

        public void SetPlan(Plany plan)
        {
            IdPlanu = plan.IdPlanu; 
        }
        public void SetPlan(long idPlanu)
        {
            IdPlanu = idPlanu;
        }

    }
}
