package ru.compscicenter.java2014.collections.test;

import org.testng.annotations.Test;
import ru.compscicenter.java2014.collections.MultiSet;

import java.lang.reflect.Constructor;
import java.util.*;

import static org.fest.assertions.api.Assertions.*;

@Test(timeOut = 1000L, sequential = true)
public class MultiSetTest {

    private Class<?> multiSetClass;

    public MultiSetTest(Class<?> multiSetClass) {
        if (multiSetClass == null) {
            throw new IllegalArgumentException("multiSetClass");
        }

        this.multiSetClass = multiSetClass;
    }

    /*
     * This is test example
     */
    private void newMultiSetMustBeEmpty() throws Exception {
        assertThat(newMultiSet()).isEmpty();
        assertThat(newMultiSet()).hasSize(0);
    }

    /*
     * This is constructor without parameters for your MultiSet implementation.
     */
    private <E> MultiSet<E> newMultiSet() throws Exception {
        Constructor<?> constructor = getNoArgConstructor();
        constructor.setAccessible(true);
        return (MultiSet<E>) constructor.newInstance();
    }

    /*
     * This is constructor with Collection parameter for your MultiSet implementation.
     */
    private <E> MultiSet<E> newMultiSet(Collection c) throws Exception {
        Constructor<?> constructor = getCollectionConstructor();
        constructor.setAccessible(true);
        return (MultiSet<E>) constructor.newInstance(c);
    }

    private Constructor<?> getNoArgConstructor() throws Exception {
        return multiSetClass.getDeclaredConstructor();
    }

    private Constructor<?> getCollectionConstructor() throws Exception {
        return multiSetClass.getDeclaredConstructor(Collection.class);
    }

    private static Integer randomInteger() {
        return (int) (Math.random() * Integer.MAX_VALUE);
    }
}
