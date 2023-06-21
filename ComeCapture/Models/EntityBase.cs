using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ComeCapture.Models
{
    /// <summary>
    /// 这个基类只作为INotifyPropertyChanged的实现，不再是Entity的基类
    /// </summary>
    public class EntityBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected virtual void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression unaryExpression)
            {
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }
            RaisePropertyChanged(memberExpression.Member.Name);
        }
        #endregion
    }
}
