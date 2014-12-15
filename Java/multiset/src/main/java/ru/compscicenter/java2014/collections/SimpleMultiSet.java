package ru.compscicenter.java2014.collections;

import java.util.Collection;
import java.util.HashMap;
import java.util.Map;
import java.util.Iterator;
import java.util.NoSuchElementException;


/**
 * Created by анастасия on 23.10.2014.
 */

public class SimpleMultiSet<E> implements MultiSet<E> {
    private HashMap<E, Integer> base;
    private int size = 0;

    public class SimpleMultiSetIterator<E> implements Iterator<E> {
        private E[] elementsOfMultiSet;
        private int currentElement;
        private int lastRemoved;

        SimpleMultiSetIterator() {
            elementsOfMultiSet = (E[]) SimpleMultiSet.this.toArray();
            lastRemoved = -1;
            currentElement = -1;
        }

        @Override
        public final boolean hasNext() {
            return currentElement < elementsOfMultiSet.length - 1;
        }

        @Override
        public final E next() {
            if (this.hasNext()) {
                ++currentElement;
                return elementsOfMultiSet[currentElement];
            } else {
                throw new NoSuchElementException();
            }

        }

        @Override
        public final void remove() {
            if (lastRemoved == currentElement) {
                throw new IllegalStateException();
            }
            try {
                lastRemoved = currentElement;
                SimpleMultiSet.this.remove(elementsOfMultiSet[currentElement]);
            }
            catch (Exception e) {
                throw new IllegalStateException();
            }
        }
    }

    public SimpleMultiSet() {
        base = new HashMap<>();
        size = 0;
    }
    public SimpleMultiSet(final Collection<? extends E> c) {
        base = new HashMap<>();
        size = 0;
        addAll(c);
    }


    @Override
    public final int size() {
        return size;
    }

    @Override
    public final Iterator iterator() {
        return new SimpleMultiSetIterator();
    }

    @Override
    public final boolean add(final E e) {
        int amount = 1;
        if (base.containsKey(e)) {
            amount += base.remove(e);
        }
        ++size;
        base.put(e, amount);
        return true;
    }

    public final int add(final E e, final int occurrences) {
        if (occurrences < 0) {
            throw new IllegalArgumentException();
        }
        int previousAmount = 0;

        if (base.containsKey(e)) {
            previousAmount = base.remove(e);
        }

        if ((previousAmount + occurrences) != 0) {
            base.put(e, previousAmount + occurrences);
        }
        size += occurrences;
        return previousAmount;
    }

    @Override
    public final boolean remove(final Object e) {
        if ((base.containsKey(e)) && (base.get(e) > 1)) {
            --size;
            base.replace((E) e, (base.get(e) - 1));
            return true;
        }
        else if ((base.containsKey(e)) && (base.get(e) == 1)) {
            --size;
            base.remove(e);
            return true;
        }
        return false;
    }

    public final int remove(final Object e, final int occurrences) {
        if (occurrences < 0) {
            throw new IllegalArgumentException();
        }

        int previousAmount = 0;
        if (base.containsKey(e)) {
            previousAmount = base.get(e);
        }

        if (previousAmount != 0) {
            if (occurrences >= previousAmount) {
                size -= previousAmount;
                base.remove(e);
            }
            else {
                size -= occurrences;
                base.replace((E) e, previousAmount - occurrences);
            }
        }
        return previousAmount;
    }

    public final int count(final Object e) {
        if ((base.containsKey(e))) {
            return base.get(e);
        }
        return 0;
    }
    public final boolean contains(final Object e) {
        return base.containsKey(e);
    }

    @Override
    public final boolean containsAll(final Collection<?> c) {
       for (Object key : c) {
           if (!base.containsKey(key)) {
               return false;
           }
       }
       return true;
    }

    public final boolean addAll(final Collection<? extends E> c) {
        boolean modified = false;
        for (E e : c) {
            if (add(e)) {
                modified = true;
            }
        }
        return modified;
    }

    @Override
    public final Object[] toArray() {
        Object[] a = new Object[size];
        int j = 0;
        for (Map.Entry<E, Integer> element : base.entrySet()) {
            E key = element.getKey();
            int value = element.getValue();
            for (int i = 0; i < value; ++i) {
                a[j++] = key;
            }
        }
        return a;
    }

    @Override
    public final <T> T[] toArray(final T[] a) {
        if (a == null) {
            throw new NullPointerException();
        }
        T[] result;
        if (a.length >= size) {
            result = a;
        }
        else {
            result = (T[]) java.lang.reflect.Array.newInstance(a.getClass().getComponentType(), size);
        }
        int j = 0;
        for (Map.Entry<E, Integer> element : base.entrySet()) {
            E key = element.getKey();
            int value = element.getValue();
            for (int i = 0; i < value; ++i) {
                try {
                    result[j++] = (T) key;
                }
                catch (ArrayStoreException ex) {
                    throw new ArrayStoreException();
                }
            }
        }

        return result;
    }

    @Override
    public final boolean removeAll(final Collection<?> c) {
        boolean isAnyRemoved = false;
        for (Object key : c) {
            if (base.containsKey(key)) {
                size -= base.get(key);
                base.remove(key);
                isAnyRemoved = true;
            }
        }
        return isAnyRemoved;
    }

    @Override
    public final boolean retainAll(final Collection<?> c) {
        boolean isAnyRemoved = false;
        for (Map.Entry<E, Integer> element : base.entrySet()) {
            E key = element.getKey();
            int value = element.getValue();
            if (!(c.contains(key))) {
                size -= base.get(key);
                base.remove(key);
                isAnyRemoved = true;
            }
        }
        return isAnyRemoved;
    }

    @Override
    public final void clear() {
        base.clear();
        size = 0;
    }

    @Override
    public final boolean equals(final Object o) {
        try {
            HashMap<E, Integer> newBase = ((SimpleMultiSet) o).base;
            for (Map.Entry<E, Integer> element : newBase.entrySet()) {
                E key = element.getKey();
                int value = element.getValue();
                if (!(base.containsKey(key) && (base.get(key) == value))) {
                    return false;
                }
            }
            for (Map.Entry<E, Integer> element : base.entrySet()) {
                E key = element.getKey();
                int value = element.getValue();
                if (!(newBase.containsKey(key) && (newBase.get(key) == value))) {
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }
    public final int hashCode() {
        return base.hashCode();
    }
    public final boolean isEmpty() {
        return base.isEmpty();
    }
}
