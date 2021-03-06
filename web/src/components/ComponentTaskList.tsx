// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import React, { useState, useEffect, useContext } from 'react';
import { CheckboxVisibility, DetailsList, DetailsListLayoutMode, FontIcon, getTheme, IColumn, IDetailsRowProps, IRenderFunction, SelectionMode, Stack, Text } from '@fluentui/react';
import { ProjectContext } from '../Context';
import { ComponentTask } from 'teamcloud';
import { useInterval } from '../Hooks';
import { api } from '../API';
import { ComponentTaskConsole } from '.';

export interface IComponentTaskListProps {

}

export const ComponentTaskList: React.FunctionComponent<IComponentTaskListProps> = (props) => {

    const theme = getTheme();

    const { component, componentTask, componentTasks, templates, onComponentTaskSelected } = useContext(ProjectContext);

    const [isPolling, setIsPolling] = useState(true);

    useEffect(() => {
        const poll = componentTask?.resourceState !== undefined
            && componentTask.resourceState.toLowerCase() !== 'succeeded'
            && componentTask.resourceState.toLowerCase() !== 'failed';

        if (isPolling !== poll) {
            console.log(`+ setPollTask (${poll})`);
            setIsPolling(poll);
        }

    }, [componentTask, isPolling])

    useInterval(async () => {
        if (componentTask) {
            console.log(`- getComponentTask (polling) (${componentTask.id})`);
            const result = await api.getComponentTask(componentTask.id, componentTask.organization, componentTask.projectId, componentTask.componentId);
            console.log(`+ getComponentTask (polling) (${componentTask.id})`);
            onComponentTaskSelected(result.data);
        }
    }, isPolling ? 3000 : undefined);

    const [dots, setDots] = useState('');

    useInterval(() => {
        const d = dots.length < 3 ? `${dots}.` : '';
        setDots(d);
    }, isPolling ? 1000 : undefined);

    const _getStateIcon = (task?: ComponentTask) => {
        if (task?.resourceState)
            switch (task.resourceState) {
                case 'Pending': return 'ProgressLoopOuter'; // UnknownSolid, AwayStatus, DRM, Blocked2
                case 'Initializing': return 'Running'; // Running, Rocket
                case 'Provisioning': return 'Rocket'; // Processing,
                case 'Succeeded': return 'CompletedSolid'; // BoxCheckmarkSolid, CheckboxComposite, Accept, CompletedSolid
                case 'Failed': return 'StatusErrorFull'; // BoxMultiplySolid, Error, ErrorBadge StatusErrorFull, IncidentTriangle
                default: return undefined;
            }
    };

    const columns: IColumn[] = [
        {
            key: 'componentId', name: 'ComponentId', minWidth: 440, maxWidth: 440, onRender: (t: ComponentTask) => (
                <Stack
                    horizontal
                    verticalAlign='center'
                    horizontalAlign='space-between'
                    tokens={{ childrenGap: '20px' }}
                    styles={{ root: { padding: '5px' } }}>
                    <Stack tokens={{ childrenGap: '6px' }}>
                        <Text styles={{ root: { color: theme.palette.neutralPrimary } }} variant='medium'>{_getTaskName(t)}</Text>
                        <Text styles={{ root: { color: theme.palette.neutralSecondary } }} variant='small'>{_getTaskStatus(t)}</Text>
                    </Stack>
                    <FontIcon iconName={_getStateIcon(t)} className={`deployment-state-icon-${t.resourceState?.toLowerCase() ?? 'pending'}`} />
                </Stack>
            )
        }
    ];

    const _onRenderRow: IRenderFunction<IDetailsRowProps> = (rowProps?: IDetailsRowProps, defaultRender?: (rowProps?: IDetailsRowProps) => JSX.Element | null): JSX.Element | null => {
        if (rowProps) rowProps.styles = {
            // root: { borderBottom: (props.noHeader ?? false) && items.length === 1 ? 0 : undefined },
            fields: { alignItems: 'center' }, check: { minHeight: '62px' }, cell: { fontSize: '14px' }
        }
        return defaultRender ? defaultRender(rowProps) : null;
    };

    const _getTaskName = (task?: ComponentTask) => {
        if (task) {
            let componentTemplate = templates?.find(t => t.id === component?.templateId);
            let taskTemplate = componentTemplate?.tasks?.find(t => t.id === task.typeName);
            return `${taskTemplate?.displayName ?? task.typeName ?? task.type}: ${task.id}`;
        }
    }

    const _getTaskStatus = (t?: ComponentTask) => {
        if (t?.resourceState) {
            if (t.resourceState.toLowerCase() === 'succeeded' || t.resourceState.toLowerCase() === 'failed') {
                return t.finished ? `${t.resourceState} ${t.finished.toLocaleString()}` : t.resourceState;
            } else {
                return `${t.resourceState} ${dots}`;
            }
        } else if (t?.started) {
            return `Started ${t.started.toLocaleString()}`;
        }
        return undefined;
    };


    return (
        <Stack
            horizontal
            verticalFill
            // horizontalAlign='stretch'
            tokens={{ childrenGap: '20px' }}
            styles={{ root: { height: '100%', padding: '24px 8px' } }}>
            <Stack.Item styles={{
                root: {
                    // minWidth: '40%'
                }
            }}>
                <DetailsList
                    items={(componentTasks ?? []).sort((a, b) => (b.created?.valueOf() ?? 0) - (a.created?.valueOf() ?? 0))}
                    columns={columns}
                    isHeaderVisible={false}
                    onRenderRow={_onRenderRow}
                    layoutMode={DetailsListLayoutMode.fixedColumns}
                    checkboxVisibility={CheckboxVisibility.hidden}
                    selectionMode={SelectionMode.single}
                    onActiveItemChanged={onComponentTaskSelected}
                    styles={{ focusZone: { minWidth: '1px' }, root: { minWidth: '460px', boxShadow: theme.effects.elevation8 } }}
                />
            </Stack.Item>
            <Stack.Item grow={2}>
                <ComponentTaskConsole task={componentTask} isPolling={isPolling} />
            </Stack.Item>
        </Stack >

    );
}
