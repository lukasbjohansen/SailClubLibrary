using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailClubLibrary.Interfaces;
public interface IRepositoryItem<K>
{
	K Key { get; set; }
	int Id { get; set; }
}
