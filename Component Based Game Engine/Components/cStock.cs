using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component_Based_Game_Engine.Components
{
    public class cStock : IComponent
    {
        private Dictionary<StockType, float> stockCounts;
        private StockClass stockClass;
        private float productionEfficiency;
        private float upgradeCostModifier;

        public cStock(StockType stockType, int stockCount)
        {
            stockCounts = new Dictionary<StockType, float>();
            stockCounts.Add(stockType, stockCount);
        }

        public cStock(StockClass stockClassIn, List<StockType> stockTypes)
        {
            stockCounts = new Dictionary<StockType, float>();
            foreach(StockType stockType in stockTypes)
            {
                stockCounts.Add(stockType, 0);
            }

            stockClass = stockClassIn;

            productionEfficiency = 1.0f;

            upgradeCostModifier = 1.0f;
        }

        public ComponentMasks ComponentMask
        {
            get { return ComponentMasks.COMPONENT_STOCK; }
        }

        public enum StockType
        {
            RAW_FOOD,
            RAW_MEDICAL,
            RAW_WOOD,
            RAW_OIL
        }

        public enum StockClass
        {
            PRODUCTION,
            DELIVERY
        }

        public Dictionary<StockType, float> StockCounts
        {
            get { return stockCounts; }
            set { stockCounts = value; }
        }

        public StockClass StockClassValue
        {
            get { return stockClass; }
            set { stockClass = value; }
        }

        public float ProductionEfficiency
        {
            get { return productionEfficiency; }
            set { productionEfficiency = value; }
        }

        public float UpgradeCost
        {
            get { return upgradeCostModifier; }
            set { upgradeCostModifier = value; }
        }
    }
}
