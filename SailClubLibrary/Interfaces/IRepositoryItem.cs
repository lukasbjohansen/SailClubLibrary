using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Interfaces;
public interface IRepositoryItem<K> : IIdAble
{
	K Key { get; set; }
}
