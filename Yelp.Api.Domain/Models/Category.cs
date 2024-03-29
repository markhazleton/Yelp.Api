﻿namespace Yelp.Api.Domain.Models;

public class Category : IEquatable<Category>
{

    public static bool operator !=(Category category1, Category category2)
    {
        if (((object)category1) == null || ((object)category2) == null)
            return !Object.Equals(category1, category2);

        return !(category1.Equals(category2));
    }

    public static bool operator ==(Category category1, Category category2)
    {
        if (((object)category1) == null || ((object)category2) == null)
            return Object.Equals(category1, category2);

        return category1.Equals(category2);
    }



    public bool Equals(Category other)
    {
        if (other == null)
            return false;

        if (this.Alias != null && this.Alias.Equals(other.Alias, StringComparison.CurrentCultureIgnoreCase))
            return true;
        else
            return false;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        Category category = obj as Category;
        if (category == null)
            return false;
        else
            return this.Equals(category);
    }

    public override int GetHashCode()
    {
        return this.Alias?.GetHashCode() ?? base.GetHashCode();
    }

    [JsonProperty("alias")]
    public string Alias { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

}