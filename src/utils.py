import os, sys, math
from datetime import datetime
import tifffile as tif
import numpy as np
import matplotlib.pyplot as plt
import matplotlib.patches as patches
import pandas as pd

def get_row_vals(centers,index_of_centers,length_of_row):
    ''' function to auto-generate row values from a single center val
    INPUTS:
        1. centers: a tuple or list of center values for a tile in Zaber units (e.g. ls = 238800, ax=47200 the input would be [238800,47200] or (238800,47200))
        2. index_of_centers: an integer indicating which tile value you're inputting (e.g. M1 = element 0, M2 = element 1... etc)
        3. length_of_row: an integer indicating how many tiles in the row (e.g. 1 if A, 4 if B... 13 if M... 1 if Q)
    OUTPUTS:
        two lists ls_row_vals and ax_row_vals
    EXAMPLE:
        M1c = (230100, 46500)
        M_ls_cs,M_ax_cs = get_row_vals(M1c,0,13)
        print(M_ls_cs)
        print(M_ax_cs)
        > [230100, 228100, 226100, 224100, 222100, 220100, 218100, 216100, 214100, 212100, 210100, 208100, 206100]
        > [46500, 67700, 88900, 110100, 131300, 152500, 173700, 194900, 216100, 237300, 258500, 279700, 300900]

    NOTE: if you have K2c, try
        K_ls_cs, K_ax_cs = get_row_vals(K2c,1,13) because K2 is the 1st element in the list of K tile values from 1-13
    '''
    ls_row_offset = -1400 #was -1400
    ax_row_offset= 21000 #was 21200
    #print('using ls_row_offset of {} and ax_row_offset of {}'.format(ls_row_offset,ax_row_offset))
    ls = centers[0]
    ax = centers[1]
    ls_row_vals=[]
    ax_row_vals=[]
    for i in np.arange(0-index_of_centers,length_of_row-index_of_centers):
        ls_row_vals.append(ls+i*ls_row_offset)
        ax_row_vals.append(ax+i*ax_row_offset)
    return ls_row_vals, ax_row_vals

def get_neighbor_tile(centers,direction):
    ''' INPUTS
        1. centers: a tuple or list of center values for a tile in Zaber units (e.g. ls = 238800, ax=47200 the input would be [238800,47200] or (238800,47200))
        2. direction: an int, either 0 for above right or 1 for below right (e.g. to get M2 from L2, use 1. to get L1 from M1, use 0)

        TODO later I should make this more flexible and harder to mess up
        OUTPUTS:
        1. new list with the center values in [ls,ax]
    '''
    ls = centers[0]
    ax = centers[1]
    M1_to_L1_offset = [-19500,9200] #was -19500, 9200
    L1_to_M2_offset = [17900,11600] #was 17600, 12200
    if direction == 0:
        new_centers = [ls+M1_to_L1_offset[0],ax+M1_to_L1_offset[1]]
        #print('generated the tile center above and right of the values provided (e.g. M1 center entered, want L1 centers) using {} offset'.format(M1_to_L1_offset))
    elif direction == 1:
        new_centers = [ls+L1_to_M2_offset[0],ax+L1_to_M2_offset[1]]
        #print('generated the tile center from below and right of the values provided (e.g. L1 center entered, want M2 centers) using {} offset'.format(L1_to_M2_offset))
    else:
        print('ERROR: you entered {}, which must be either 0 (for above, right) or 1 (for below, right)')
        new_centers=[]
    return new_centers

def get_relative_vals(df, location1, location2, adj_val):
    '''
    INPUTS:
        1. df: a pandas DataFrame with three columns: name, ls_guess, and ax_new_guess
        2.
        3.
        4. adj_val = adjust the outputs by this amount
    OUTPUTS:
        1. ls_high the highest ls value as an integer
        2. ls_low the lowest ls value as an integer
        3. ax_high the highest ax value as an integer
        4. ax_low the lowest ax value as an integer
    '''
    ax1= int(df.ax_new_guess[df.name==location1]/100)
    ax2 = int(df.ax_new_guess[df.name==location2]/100)
    ls1= int(df.ls_guess[df.name==location1]/100)
    ls2 = int(df.ls_guess[df.name==location2]/100)
    if ls1 > ls2:
        ls_high = ls1
        ls_low = ls2
    else:
        ls_low = ls1
        ls_high = ls2
    if ax1 > ax2:
        ax_low = ax2
        ax_high = ax1
    else:
        ax_low = ax1
        ax_high = ax2
    return ls_high+adj_val,ls_low-adj_val,ax_high+adj_val,ax_low-adj_val


def get_closest(list_input,num):
    aux = []
    for val in list_input:
        aux.append(abs(num-val))

    return aux.index(min(aux))

def getpoints(p1, p2):
    # Sort both points first.
    (x1, y1), (x2, y2) = sorted([p1, p2])
    a = b = 0.0
    yvals=[]
    xvals=np.arange(int(x1), int(x2) + 1)
    # First point is in (0, y).
    if x1 == 0.0:
        b = y1
        a = (y2 - y1) / x2
    elif x2 == 0.0:
        # Second point is in (0, y).
        b = y2
        a = (y1 - y2) / x1
    else:
        # Both points are valid.
        b = (y2 - (y1 * x2) / x1) / (1 - (x2 / x1))
        a = (y1 - b) / x1
        for x in xvals:
            yvals.append(a * float(x) + b)
    yvals=[round(val) for val in yvals]
    axs=[]
    lss=[]
    for val in np.arange(np.min(yvals),np.max(yvals)):
        i=get_closest(yvals,val) # gets integer of yval closest
        lss.append(xvals[i])
        axs.append(yvals[i])
    return (lss, axs)

def rot_list_of_ax_ls(ax_list,ls_list,angle):
    # first check lists are same length
    if len(ax_list)!=len(ls_list):
        print('ERROR, your lists are two different lengths! 1st argument, ax_list is {} and 2nd argument ls_list is {}'.format(len(ax_list),len(ls_list)))
    else:
        ax_new=[]
        ls_new=[]

        ax_com=np.mean(ax_list)
        ls_com = np.mean(ls_list)
        ax_corr = ax_list-ax_com
        ls_corr = ls_list-ls_com
        print('using angle of {} degrees'.format(angle))
        alfa = angle*math.pi/180
        for i in range(len(ax_list)):
            ax_new.append((math.cos(alfa)*ax_corr[i]-math.sin(alfa)*ls_corr[i])+ax_com)
            ls_new.append((math.sin(alfa)*ax_corr[i]+math.cos(alfa)*ls_corr[i])+ls_com)
    return [ax_new,ls_new]
