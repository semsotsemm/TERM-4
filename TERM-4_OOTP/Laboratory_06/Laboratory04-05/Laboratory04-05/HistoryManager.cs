using System;
using System.Collections.Generic;
using System.Linq;

namespace Laboratory04_05
{
    public interface IHistoryAction
    {
        void Undo();
        void Redo();
    }

    public static class HistoryManager
    {
        public static Stack<IHistoryAction> UndoStack = new Stack<IHistoryAction>();
        public static Stack<IHistoryAction> RedoStack = new Stack<IHistoryAction>();

        public static void AddAction(IHistoryAction action)
        {
            UndoStack.Push(action);
            RedoStack.Clear(); 
        }

        public static void Undo()
        {
            if (UndoStack.Count > 0)
            {
                var action = UndoStack.Pop();
                action.Undo();
                RedoStack.Push(action);
            }
        }

        public static void Redo()
        {
            if (RedoStack.Count > 0)
            {
                var action = RedoStack.Pop();
                action.Redo();
                UndoStack.Push(action);
            }
        }
    }

    public class BasketToggleAction : IHistoryAction
    {
        private string _productName;
        private bool _wasAddedToBasket;
        private ProductManager _prodManager = new ProductManager();
        private UserManager _userManager = new UserManager();

        public BasketToggleAction(string productName, bool wasAddedToBasket)
        {
            _productName = productName;
            _wasAddedToBasket = wasAddedToBasket;
        }

        public void Undo()
        {
            if (_wasAddedToBasket) RemoveFromBasket(); else AddToBasket();
        }

        public void Redo()
        {
            if (_wasAddedToBasket) AddToBasket(); else RemoveFromBasket();
        }

        private void AddToBasket()
        {
            var product = _prodManager.getProductsList().FirstOrDefault(p => p.Name == _productName);
            if (product != null)
            {
                var oldState = Clone(product);
                product.Quantity -= 1;
                _prodManager.EditProduct(oldState, product);

                Session.CurrentUser.basket.Add(product.Name);
                _userManager.UpdateUser(Session.CurrentUser);
            }
        }

        private void RemoveFromBasket()
        {
            var product = _prodManager.getProductsList().FirstOrDefault(p => p.Name == _productName);
            if (product != null)
            {
                var oldState = Clone(product);
                product.Quantity += 1;
                _prodManager.EditProduct(oldState, product);

                Session.CurrentUser.basket.Remove(product.Name);
                _userManager.UpdateUser(Session.CurrentUser);
            }
        }

        private ProductManager.Product Clone(ProductManager.Product p)
        {
            var clone = new ProductManager.Product();
            foreach (var prop in typeof(ProductManager.Product).GetProperties())
                if (prop.CanWrite) prop.SetValue(clone, prop.GetValue(p));
            return clone;
        }
    }
}