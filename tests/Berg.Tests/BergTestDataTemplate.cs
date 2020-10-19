using Berg.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berg.Tests {
    public abstract class BergTestDataTemplate {

        protected List<Item> ITEM_LIST = new List<Item>() {
                    new Item("itemOne", 1.00M),
                    new Item("itemTwo", 2.50M),
                    new Item("itemThree", 3.33M)
                };

    }
}
