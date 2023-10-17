using Demo.Models;

namespace Demo.ModelsView
{
	public class CartItem
	{
		public Sach product { get; set; }
		public int amount { get; set; }
		public double TongTien => amount * product.Gia.Value;
	}
}
